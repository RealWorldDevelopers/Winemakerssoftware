
using WMS.Ui.Models.Admin;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using WMS.Ui.Models;
using Microsoft.Extensions.Options;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.Extensions.Configuration;
using AutoMapper;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using WMS.Business.Shared;
using WMS.Business.Yeast.Dto;
using WMS.Business.Recipe.Dto;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace WMS.Ui.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : BaseController
    {
        private readonly Models.Admin.IFactory _modelFactory;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        private readonly Data.ApplicationDbContext _context;
        private readonly Business.Recipe.Queries.IFactory _recipeQueryFactory;
        private readonly Business.Recipe.Commands.IFactory _recipeCommandFactory;
        private readonly Business.Yeast.Queries.IFactory _yeastQueryFactory;
        private readonly Business.Yeast.Commands.IFactory _yeastCommandFactory;

        public AdminController(IConfiguration configuration, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager,
            Business.Recipe.Queries.IFactory recipeQueryFactory, Business.Recipe.Commands.IFactory recipeCommandFactory, Business.Yeast.Queries.IFactory yeastQueryFactory,
            Business.Yeast.Commands.IFactory yeastCommandFactory, Data.ApplicationDbContext context, IMapper mapper, Models.Admin.IFactory modelFactory,
            IOptions<AppSettings> appSettings) : base(configuration, userManager, roleManager)
        {
            _modelFactory = modelFactory;
            _appSettings = appSettings.Value;
            _mapper = mapper;
            _context = context;
            _recipeQueryFactory = recipeQueryFactory;
            _recipeCommandFactory = recipeCommandFactory;
            _yeastQueryFactory = yeastQueryFactory;
            _yeastCommandFactory = yeastCommandFactory;
        }

        public async Task<IActionResult> Index(string id)
        {
            ViewData["Title"] = "Administration";

            var getYeastQuery = _yeastQueryFactory.CreateYeastsQuery();
            var getYeastPairs = _yeastQueryFactory.CreateYeastPairQuery();
            var getCategoriesQuery = _recipeQueryFactory.CreateCategoriesQuery();
            var getVarietiesQuery = _recipeQueryFactory.CreateVarietiesQuery();
            var getRecipesQuery = _recipeQueryFactory.CreateRecipesQuery();

            // using TPL to parallel call gets
            List<Task> tasks = new List<Task>();

            var t1 = Task.Run(async () => await getCategoriesQuery.ExecuteAsync());
            tasks.Add(t1);
            var cList = await t1;

            var t2 = Task.Run(async () => await getVarietiesQuery.ExecuteAsync());
            tasks.Add(t2);
            var vList = await t2;

            var t3 = Task.Run(async () => await getYeastQuery.ExecuteAsync());
            tasks.Add(t3);
            var yList = await t3;

            var t4 = Task.Run(async () => await getYeastPairs.ExecuteAsync());
            tasks.Add(t4);
            var ypList = await t4;

            var t5 = Task.Run(async () => await getRecipesQuery.ExecuteAsync());
            tasks.Add(t5);
            var rList = await t5;

            Task.WaitAll(tasks.ToArray());



            var model = _modelFactory.CreateAdminModel(id);

            // make sure admin security role exist
            if (!await _roleManager.RoleExistsAsync(_appSettings.SecRole.Admin))
            {
                ApplicationRole role = new ApplicationRole
                {
                    Name = _appSettings.SecRole.Admin,
                    Description = "Perform all operations."
                };
                IdentityResult roleResult = await _roleManager.CreateAsync(role);
                if (!roleResult.Succeeded)
                {
                    ModelState.AddModelError(string.Empty, "Error while creating role!");
                    return View(model);
                }
            }

            // gather users data
            var users = _userManager.Users.ToList();
            model.UsersViewModel.Users = _mapper.Map<List<UserViewModel>>(users);
            foreach (var user in model.UsersViewModel.Users)
            {
                user.IsAdmin = await _userManager.IsInRoleAsync(user, _appSettings.SecRole.Admin);
                user.IsLockedOut = await _userManager.IsLockedOutAsync(user);
            }

            // gather roles data
            var roles = await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();
            model.RolesViewModel.Roles = _mapper.Map<List<RoleViewModel>>(roles);

            // gather category / variety data    
            model.CategoriesViewModel.Categories = _modelFactory.CreateCategoryViewModel(cList);
            model.VarietiesViewModel.Varieties = _modelFactory.CreateVarietyViewModel(vList);

            // gather yeast data   
            model.YeastsViewModel.Yeasts = _modelFactory.CreateYeastViewModel(yList);

            // gather recipe data   
            model.RecipesViewModel.Recipes = _modelFactory.CreateRecipeViewModel(rList);

            return View(model);

        }

        #region Recipes           

        /// <summary>
        /// Main entry page to edit a Recipe
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRecipe(int Id)
        {
            ViewData["Title"] = "Edit a Recipe";

            var recipeQry = _recipeQueryFactory.CreateRecipesQuery();
            var dto = await recipeQry.ExecuteAsync(Id);
            var model = _modelFactory.CreateRecipeViewModel(dto);

            var user = await _userManager.FindByIdAsync(dto.SubmittedBy);
            model.SubmittedBy = string.Concat(user.FirstName, " ", user.LastName, " (", user.Email, ")");

            return View("UpdateRecipe", model);
        }

        /// <summary>
        /// Update or Add a new Recipe in the database
        /// </summary>
        /// <param name="model">Recipe as <see cref="RecipeViewModel"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateRecipe(RecipeViewModel model)
        {
            var qry = _recipeQueryFactory.CreateRecipesQuery();
            var dto = await qry.ExecuteAsync(model.Id);
            dto.Title = model.Title;
            dto.Variety.Id = model.Variety.Id;
            dto.Description = model.Description;
            dto.Enabled = model.Enabled;
            dto.Hits = model.Hits;
            dto.Ingredients = model.Ingredients;
            dto.Instructions = model.Instructions;
            dto.NeedsApproved = model.NeedsApproved;

            var cmd = _recipeCommandFactory.CreateRecipesCommand();
            await cmd.UpdateAsync(dto);

            return RedirectToAction("Index", "Admin", new { id = "recipes" });
        }

        /// <summary>
        /// Delete a Recipe from the database
        /// </summary>
        /// <param name="Id"> Id of Recipe to delete as <see cref="int"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRecipe(int Id)
        {
            var cmd = _recipeCommandFactory.CreateRecipesCommand();
            var qry = _recipeQueryFactory.CreateRecipesQuery();
            var dto = await qry.ExecuteAsync(Id);
            await cmd.DeleteAsync(dto);
            return RedirectToAction("Index", "Admin", new { id = "recipes" });
        }

        /// <summary>
        /// Delete an Image and Remove from Recipe Map table
        /// </summary>
        /// <param name="recipeId">Recipe Id as <see cref="int"/></param>
        /// <param name="imageId">Image Id as <see cref="int"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRecipeImage(int recipeId, int imageId)
        {
            var updateImageCommand = _recipeCommandFactory.CreateImageCommand();
            var imageDto = new ImageFile
            {
                Id = imageId,
                RecipeId = recipeId
            };
            await updateImageCommand.DeleteAsync(imageDto);
            return await EditRecipe(recipeId);
        }

        /// <summary>
        /// Add an Image and Map it to a Recipe
        /// </summary>
        /// <param name="recipeId">REcipe Id as <see cref="int"/></param>
        /// <param name="image">Inbound Image File as <see cref="IFormFile"/></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRecipeImage(int recipeId, IFormFile image)
        {
            if (image != null)
            {
                var updateImageCommand = _recipeCommandFactory.CreateImageCommand();
                long maxFileSizeBytes = 512000;
                List<string> allowedExtensions = new List<string> { ".jpg", ".jpeg", ".bmp", ".png", ".gif" };

                // Max File Size per Image: 500 KB
                if (image.Length > maxFileSizeBytes)
                {
                    Danger("File Too Big", true);
                    return await EditRecipe(recipeId);
                }

                // Allowed Image Extensions: .jpg | .gif | .bmp | .jpeg | .png ONLY
                var ext = Path.GetExtension(image.FileName);
                if (!allowedExtensions.Any(e => e.Equals(ext, StringComparison.OrdinalIgnoreCase)))
                {
                    Danger("File Extension Not Supported", true);
                    return await EditRecipe(recipeId);
                }

                MemoryStream ms = new MemoryStream();
                image.OpenReadStream().CopyTo(ms);
                var imageData = await ResizeImage(ms.ToArray(), 360, 480);
                var thumbData = await ResizeImage(ms.ToArray(), 100, 150);

                var imageDto = new ImageFile
                {
                    RecipeId = recipeId,
                    FileName = image.FileName,
                    Name = image.Name,
                    Data = imageData,   
                    Thumbnail=thumbData,
                    Length = image.Length,
                    ContentType = image.ContentType
                };

                await updateImageCommand.AddAsync(imageDto);
            }

            return await EditRecipe(recipeId);
        }

        #endregion

        #region Yeasts

        /// <summary>
        /// Main entry page to enter a yeast
        /// </summary>
        public IActionResult AddYeast()
        {
            ViewData["Title"] = "Add a Yeast";

            var model = _modelFactory.CreateYeastViewModel();
            return View("UpdateYeast", model);
        }

        /// <summary>
        /// Main entry page to edit a Yeast
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditYeast(int Id)
        {
            ViewData["Title"] = "Edit a Yeast";

            var yQry = _yeastQueryFactory.CreateYeastsQuery();
            var dto = await yQry.ExecuteAsync(Id);
            var model = _modelFactory.CreateYeastViewModel(dto);
            return View("UpdateYeast", model);
        }

        /// <summary>
        /// Update or Add a new Yeast in the database
        /// </summary>
        /// <param name="model">Yeast as <see cref="YeastViewModel"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateYeast(YeastViewModel model)
        {
            var dto = _mapper.Map<Yeast>(model);
            var cmd = _yeastCommandFactory.CreateYeastsCommand();
            if (dto.Id == 0)
                await cmd.AddAsync(dto);
            else
                await cmd.UpdateAsync(dto);

            return RedirectToAction("Index", "Admin", new { id = "yeasts" });
        }

        /// <summary>
        /// Delete a Yeast from the database
        /// </summary>
        /// <param name="Id"> Id of Yeast to delete as <see cref="int"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteYeast(int Id)
        {
            var cmd = _yeastCommandFactory.CreateYeastsCommand();
            var qry = _yeastQueryFactory.CreateYeastsQuery();
            var dto = await qry.ExecuteAsync(Id);
            await cmd.DeleteAsync(dto);
            return RedirectToAction("Index", "Admin", new { id = "yeasts" });
        }

        /// <summary>
        /// Main entry page to edit a Yeast Pairing
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditYeastPairing(int Id)
        {
            ViewData["Title"] = "Edit a Yeast";

            var pQry = _yeastQueryFactory.CreateYeastPairQuery();
            var pDto = await pQry.ExecuteAsync(Id);

            var yQry = _yeastQueryFactory.CreateYeastsQuery();
            var yDto = await yQry.ExecuteAsync(pDto.Yeast.Value);
            var model = _modelFactory.CreateYeastViewModel(yDto);
            model.Pairing = _modelFactory.CreateYeastPairingViewModel(pDto);

            return View("UpdateYeast", model);
        }

        /// <summary>
        /// Update or Add a new Yeast Pairing in the database
        /// </summary>
        /// <param name="model">Yeast as <see cref="YeastPairingViewModel"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateYeastPairing(YeastPairingViewModel model)
        {
            var cQuery = _recipeQueryFactory.CreateCategoriesQuery();
            var vQuery = _recipeQueryFactory.CreateVarietiesQuery();

            var dto = new YeastPair();
            var variety = await vQuery.ExecuteAsync(model.Variety.Id);
            if (variety != null)
            {
                dto.Variety = variety.Id;
                dto.Category = variety.ParentId;
            }
            else
            {
                var cat = await cQuery.ExecuteAsync(model.Variety.Id);
                dto.Category = cat.Id;
                dto.Yeast = model.Yeast.Id;
            }
            dto.Id = model.Id;
            dto.Yeast = model.Yeast.Id;
            dto.Note = model.Note;

            var cmd = _yeastCommandFactory.CreateYeastPairCommand();
            if (dto.Id == 0)
                await cmd.AddAsync(dto);
            else
                await cmd.UpdateAsync(dto);

            return RedirectToAction("Index", "Admin", new { id = "yeasts" });
        }

        /// <summary>
        /// Delete a Pairing from the database
        /// </summary>
        /// <param name="Id"> Id of Pairing to delete as <see cref="int"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePairing(int Id)
        {
            var cmd = _yeastCommandFactory.CreateYeastPairCommand();
            var qry = _yeastQueryFactory.CreateYeastPairQuery();
            var dto = await qry.ExecuteAsync(Id);
            await cmd.DeleteAsync(dto);
            return RedirectToAction("Index", "Admin", new { id = "yeasts" });
        }

        #endregion

        #region Varieties

        /// <summary>
        /// Main entry page to enter a Variety
        /// </summary>
        public IActionResult AddVariety()
        {
            ViewData["Title"] = "Add a Variety";

            var model = _modelFactory.CreateVarietyViewModel();
            return View("UpdateVariety", model);
        }

        /// <summary>
        /// Main entry page to edit a Variety
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditVariety(int Id)
        {
            ViewData["Title"] = "Edit a Variety";

            var vQry = _recipeQueryFactory.CreateVarietiesQuery();
            var dto = await vQry.ExecuteAsync(Id);
            var cQry = _recipeQueryFactory.CreateCategoriesQuery();
            var cats = await cQry.ExecuteAsync();
            var model = _modelFactory.CreateVarietyViewModel(dto, cats.FirstOrDefault(c => c.Id == dto.ParentId));
            return View("UpdateVariety", model);
        }

        /// <summary>
        /// Delete a Variety from the database
        /// </summary>
        /// <param name="Id"> Id of Variety to delete as <see cref="int"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVariety(int Id)
        {
            var cmd = _recipeCommandFactory.CreateVarietiesCommand();
            var qry = _recipeQueryFactory.CreateVarietiesQuery();
            var dto = await qry.ExecuteAsync(Id);
            await cmd.DeleteAsync(dto);
            return RedirectToAction("Index", "Admin", new { id = "varieties" });
        }

        /// <summary>
        /// Update or Add a new Variety in the database
        /// </summary>
        /// <param name="model">Variety as <see cref="VarietyViewModel"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVariety(VarietyViewModel model)
        {
            var dto = _mapper.Map<ICode>(model);
            var cmd = _recipeCommandFactory.CreateVarietiesCommand();
            if (dto.Id == 0)
                await cmd.AddAsync(dto);
            else
                await cmd.UpdateAsync(dto);

            return RedirectToAction("Index", "Admin", new { id = "varieties" });
        }

        #endregion

        #region Categories

        /// <summary>
        /// Main entry page to enter a category
        /// </summary>
        public IActionResult AddCategory()
        {
            ViewData["Title"] = "Add a Category";

            var model = new CategoryViewModel();
            return View("UpdateCategory", model);
        }

        /// <summary>
        /// Main entry page to edit a category
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(int Id)
        {
            ViewData["Title"] = "Edit a Category";

            var qry = _recipeQueryFactory.CreateCategoriesQuery();
            var dto = await qry.ExecuteAsync(Id);
            var model = _modelFactory.CreateCategoryViewModel(dto);
            return View("UpdateCategory", model);
        }

        /// <summary>
        /// Delete a category from the database
        /// </summary>
        /// <param name="Id"> Id of Category to delete as <see cref="int"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int Id)
        {
            var cmd = _recipeCommandFactory.CreateCategoriesCommand();
            var qry = _recipeQueryFactory.CreateCategoriesQuery();
            var dto = await qry.ExecuteAsync(Id);
            await cmd.DeleteAsync(dto);
            return RedirectToAction("Index", "Admin", new { id = "categories" });
        }

        /// <summary>
        /// Update or Add a new category in the database
        /// </summary>
        /// <param name="model">Category as <see cref="CategoryViewModel"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCategory(CategoryViewModel model)
        {
            var dto = _mapper.Map<ICode>(model);
            var cmd = _recipeCommandFactory.CreateCategoriesCommand();
            if (dto.Id == 0)
                await cmd.AddAsync(dto);
            else
                await cmd.UpdateAsync(dto);

            return RedirectToAction("Index", "Admin", new { id = "categories" });
        }

        #endregion

        #region Users

        /// <summary>
        /// Lock out a User from the application
        /// </summary>
        /// <param name="UserName">User's Username property value as <see cref="string"/></param>
        /// <param name="timeOut">Amount of time to keep user locked out as <see cref="DateTimeOffset"/> (default value is forever)</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LockUser(string UserName, DateTimeOffset? timeOut = null)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            if (!await _userManager.IsLockedOutAsync(user))
            {
                var result = await _userManager.SetLockoutEnabledAsync(user, true);
                if (result.Succeeded)
                {
                    if (timeOut.HasValue)
                        result = await _userManager.SetLockoutEndDateAsync(user, timeOut);
                    else
                        result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.MaxValue);
                }
            }

            return RedirectToAction("Index", "Admin", new { id = "users" });
        }

        /// <summary>
        /// Unlock a User who is currently locked out
        /// </summary>
        /// <param name="UserName">User's Username property value as <see cref="string"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UnlockUser(string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            if (await _userManager.IsLockedOutAsync(user))
            {
                var result = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow);
                if (result.Succeeded)
                {
                    await _userManager.ResetAccessFailedCountAsync(user);
                }
            }

            return RedirectToAction("Index", "Admin", new { id = "users" });
        }

        /// <summary>
        /// Delete User from the Identity Store
        /// </summary>
        /// <param name="userName">User's Username property value as <see cref="string"/></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string UserName)
        {
            var user = await _userManager.FindByNameAsync(UserName);
            if (user != null)
            {
                if (!await _userManager.IsInRoleAsync(user, _appSettings.SecRole.Admin))
                    await _userManager.DeleteAsync(user);
            }

            return RedirectToAction("Index", "Admin", new { id = "users" });
        }


        /// <summary>
        /// Main entry page to edit a User
        /// </summary>
        /// <param name="model">Information on User and Role as <see cref="UserViewModel"/></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UserViewModel model)
        {
            ViewData["Title"] = "Edit a User";

            var user = await _userManager.FindByNameAsync(model.UserName);
            model = _mapper.Map<UserViewModel>(user);
            model.MemberRoles = await _userManager.GetRolesAsync(user);
            model.AllRoles = GetAllRolesAsSelectList();

            return View(model);
        }

        /// <summary>
        /// Update User information
        /// </summary>
        /// <param name="model">Information on User and Role as <see cref="UserViewModel"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(UserViewModel model)
        {
            ViewData["Title"] = "Edit a User";

            var user = await _userManager.FindByNameAsync(model.UserName);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            await _userManager.UpdateAsync(user);
            model = _mapper.Map<UserViewModel>(user);
            model.MemberRoles = await _userManager.GetRolesAsync(user);
            model.AllRoles = GetAllRolesAsSelectList();

            return View("EditUser", model);

        }

        /// <summary>
        /// Add a User to the membership of a role
        /// </summary>
        /// <param name="model">Information on User and Role as <see cref="UserViewModel"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUserRole(UserViewModel model)
        {
            ViewData["Title"] = "Add a User Role";

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (await _roleManager.RoleExistsAsync(model.NewRole) && !await _userManager.IsInRoleAsync(user, model.NewRole))
                await _userManager.AddToRoleAsync(user, model.NewRole);
            model = _mapper.Map<UserViewModel>(user);
            model.MemberRoles = await _userManager.GetRolesAsync(user);
            model.AllRoles = GetAllRolesAsSelectList();

            return View("EditUser", model);

        }

        /// <summary>
        /// Delete membership of a user to a role
        /// </summary>
        /// <param name="model">Information on User and Role as <see cref="UserViewModel"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserRole(UserViewModel model)
        {
            ViewData["Title"] = "Edit a User";

            var user = await _userManager.FindByNameAsync(model.UserName);
            if (await _roleManager.RoleExistsAsync(model.NewRole) && await _userManager.IsInRoleAsync(user, model.NewRole))
                await _userManager.RemoveFromRoleAsync(user, model.NewRole);
            model = _mapper.Map<UserViewModel>(user);
            model.MemberRoles = await _userManager.GetRolesAsync(user);
            model.AllRoles = GetAllRolesAsSelectList();

            return View("EditUser", model);
        }



        #endregion

        #region Roles

        /// <summary>
        /// Main entry page to enter a role
        /// </summary>
        [HttpGet]
        public IActionResult AddRole()
        {
            ViewData["Title"] = "Create a New Role";

            var model = new ApplicationRole();
            return View(model);
        }

        /// <summary>
        /// Add a role to the identity store
        /// </summary>
        /// <param name="model">New role as <see cref="ApplicationRole"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddRole(ApplicationRole model)
        {
            // Create Role
            if (!await _roleManager.RoleExistsAsync(model.Name))
            {
                IdentityResult roleResult = await _roleManager.CreateAsync(model);
                if (!roleResult.Succeeded)
                {
                    Danger("Error while creating role!", true);
                    return View(model);
                }
            }

            return RedirectToAction("Index", "Admin", new { id = "roles" });
        }

        /// <summary>
        /// Delete a role from the identity store
        /// </summary>
        /// <param name="roleName">Name of role to delete as <see cref="string"/></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteRole(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role != null && role.Name != _appSettings.SecRole.Admin)
            {
                IdentityResult roleResult = await _roleManager.DeleteAsync(role);
                if (!roleResult.Succeeded)
                    throw new Exception("Error while creating role!");

            }

            return RedirectToAction("Index", "Admin", new { id = "roles" });
        }


        #endregion


        /// <summary>
        /// Get all available roles for display
        /// </summary>
        /// <returns>All roles as <see cref="List{SelectListItem}}"/></returns>
        private List<SelectListItem> GetAllRolesAsSelectList()
        {
            List<SelectListItem> SelectRoleListItems =
                new List<SelectListItem>();

            var roles = _roleManager.Roles.OrderBy(x => x.Name).ToList();

            SelectRoleListItems.Add(
                new SelectListItem
                {
                    Text = "Select",
                    Value = "0"
                });

            foreach (var item in roles)
            {
                SelectRoleListItems.Add(
                    new SelectListItem
                    {
                        Text = item.Name.ToString(),
                        Value = item.Name.ToString()
                    });
            }

            return SelectRoleListItems;
        }

    }
}