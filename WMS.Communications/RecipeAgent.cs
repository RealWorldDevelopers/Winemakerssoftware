
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using WMS.Domain;

namespace WMS.Communications
{
    public class RecipeAgent : IRecipeAgent
    {
        private readonly HttpClient _httpClient;

        public RecipeAgent(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<IEnumerable<Recipe>> GetRecipes()
        {
            try
            {
                List<Recipe> model = new();

                var responseMessage = await _httpClient.GetAsync("api/recipes").ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var list = JsonConvert.DeserializeObject<List<Recipe>>(jsonString, jsonSerializerSettings);
                    if (list != null)
                        model = list;
                }
                else
                {
                    // TODO handle error
                    var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    await Task.Delay(10);

                    //await _logger.WarnAsync("Unsuccessful ResponseMessage", "TransactionAgent.GetAsync() - Unsuccessful",
                    //   new Dictionary<string, object>
                    //   {
                    // { "RequestMessage", responseMessage.RequestMessage.ToString() },
                    // { "StatusCode",responseMessage.StatusCode },
                    // { "ReasonPhrase",responseMessage.ReasonPhrase },
                    // { "Content",responseString }
                    //   });
                }
                return model;
            }
            catch (Exception e)
            {
                // TODO await _logger.ErrorAsync(e, "Communication.TransactionAgent.GetAsync()");
                throw;
            }
        }
               
        public async Task<Recipe> GetRecipe(int id)
        {
            try
            {
                Recipe model = new();

                var responseMessage = await _httpClient.GetAsync($"api/recipes/{id}").ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var obj = JsonConvert.DeserializeObject<Recipe>(jsonString, jsonSerializerSettings);
                    if (obj != null)
                        model = obj;
                }
                else
                {
                    // TODO add logging
                    var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    await Task.Delay(10);

                    //await _logger.WarnAsync("Unsuccessful ResponseMessage", "TransactionAgent.GetAsync() - Unsuccessful",
                    //   new Dictionary<string, object>
                    //   {
                    // { "RequestMessage", responseMessage.RequestMessage.ToString() },
                    // { "StatusCode",responseMessage.StatusCode },
                    // { "ReasonPhrase",responseMessage.ReasonPhrase },
                    // { "Content",responseString }
                    //   });
                }
                return model;
            }
            catch (Exception e)
            {
                // TODO await _logger.ErrorAsync(e, "Communication.TransactionAgent.GetAsync()");
                throw;
            }
        }

        public async Task<Recipe> AddRecipe(Recipe recipe)
        {
            try
            {
                Recipe? model = null;
                                
                var httpContent = new StringContent(JsonConvert.SerializeObject(recipe), Encoding.UTF8, "application/json");
                var responseMessage = await _httpClient.PostAsync($"api/recipes", httpContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    model = JsonConvert.DeserializeObject<Recipe>(jsonString, jsonSerializerSettings);                 
                }
                else
                {
                    // TODO add logging
                    var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    await Task.Delay(10);

                    //await _logger.WarnAsync("Unsuccessful ResponseMessage", "TransactionAgent.AddAsync(TransactionsModel dtoList) - Unsuccessful",
                    //   new Dictionary<string, object>
                    //   {
                    // { "RequestMessage", responseMessage.RequestMessage.ToString() },
                    // { "StatusCode",responseMessage.StatusCode },
                    // { "ReasonPhrase",responseMessage.ReasonPhrase },
                    // { "Content",responseString }
                    //   });
                }
                return model ?? recipe;
            }
            catch (Exception e)
            {
                // TODO await _logger.ErrorAsync(e, "Communication.TransactionAgent.AddAsync(TransactionsModel dtoList)");
                throw;
            }
        }

        public async Task<Recipe> UpdateRecipe(Recipe recipe)
        {
            try
            {
                var httpContent = new StringContent(JsonConvert.SerializeObject(recipe), Encoding.UTF8, "application/json");
                var responseMessage = await _httpClient.PutAsync($"api/recipes/{recipe.Id}", httpContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var model = JsonConvert.DeserializeObject<Recipe>(jsonString, jsonSerializerSettings);
                    return model ?? recipe;
                }
                else
                {
                    // TODO add logging
                    var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    await Task.Delay(10);

                    // await _logger.WarnAsync("Unsuccessful ResponseMessage", "TransactionAgent.UpdateAsync(TransactionsModel dto) - Unsuccessful",
                    //   new Dictionary<string, object>
                    //   {
                    // { "RequestMessage", responseMessage.RequestMessage.ToString() },
                    // { "StatusCode",responseMessage.StatusCode },
                    // { "ReasonPhrase",responseMessage.ReasonPhrase },
                    // { "Content",responseString }
                    //   });
                }

                return recipe;
            }
            catch (Exception e)
            {
                // TODO await _logger.ErrorAsync(e, "Communication.TransactionAgent.UpdateAsync(TransactionsModel dto)");
                throw;
            }
        }

        public async Task<bool> DeleteRecipe(int id)
        {
            try
            {
                var responseMessage = await _httpClient.DeleteAsync($"api/recipes/{id}");
                return responseMessage.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                // TODO await _logger.ErrorAsync(e, "Communication.TransactionAgent.DeleteAsync(int id)");
                throw;
            }

        }


    }

}
