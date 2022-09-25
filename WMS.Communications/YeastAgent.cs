
using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;
using WMS.Domain;

namespace WMS.Communications
{
    public class YeastAgent : IYeastAgent
    {
        private readonly HttpClient _httpClient;

        public YeastAgent(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Yeast> AddYeast(Yeast yeast)
        {
            try
            {
                Yeast model = new();

                var httpContent = new StringContent(JsonConvert.SerializeObject(yeast), Encoding.UTF8, "application/json");
                var responseMessage = await _httpClient.PostAsync($"api/yeasts", httpContent).ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var obj = JsonConvert.DeserializeObject<Yeast>(jsonString, jsonSerializerSettings);
                    if (obj != null)
                        model = obj;
                }
                else
                {
                    // TODO handle error
                    var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    await Task.Delay(10);

                    //var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    //await _logger.WarnAsync("Unsuccessful ResponseMessage", "TransactionAgent.AddAsync(TransactionsModel dtoList) - Unsuccessful",
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
                // await _logger.ErrorAsync(e, "Communication.TransactionAgent.AddAsync(TransactionsModel dtoList)");
                throw;
            }
        }

        public async Task<bool> DeleteYeast(int id)
        {
            try
            {
                var responseMessage = await _httpClient.DeleteAsync($"api/yeasts/{id}").ConfigureAwait(false);
                return responseMessage.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                //await _logger.ErrorAsync(e, "Communication.TransactionAgent.DeleteAsync(int id)");
                throw;
            }
        }

        public async Task<IEnumerable<Yeast>> GetYeasts()
        {
            try
            {
                IEnumerable<Yeast> model = new List<Yeast>();

                var responseMessage = await _httpClient.GetAsync("api/yeasts").ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var list = JsonConvert.DeserializeObject<List<Yeast>>(jsonString, jsonSerializerSettings);
                    if (list != null)
                        model = list;
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

        public async Task<Yeast> GetYeast(int id)
        {
            try
            {
                Yeast model = new();

                var responseMessage = await _httpClient.GetAsync($"api/Yeasts/{id}").ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var obj = JsonConvert.DeserializeObject<Yeast>(jsonString, jsonSerializerSettings);
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

        public async Task<Yeast> UpdateYeast(Yeast yeast)
        {
            try
            {
                Yeast? model = null;

                var httpContent = new StringContent(JsonConvert.SerializeObject(yeast), Encoding.UTF8, "application/json");
                var responseMessage = await _httpClient.PutAsync($"api/yeasts/{yeast.Id}", httpContent).ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var obj = JsonConvert.DeserializeObject<Yeast>(jsonString, jsonSerializerSettings);
                    if (obj != null)
                        model = obj;
                }
                else
                {
                    // TODO handle error
                    var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    await Task.Delay(10);

                    //var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    //await _logger.WarnAsync("Unsuccessful ResponseMessage", "TransactionAgent.UpdateAsync(TransactionsModel dto) - Unsuccessful",
                    //   new Dictionary<string, object>
                    //   {
                    // { "RequestMessage", responseMessage.RequestMessage.ToString() },
                    // { "StatusCode",responseMessage.StatusCode },
                    // { "ReasonPhrase",responseMessage.ReasonPhrase },
                    // { "Content",responseString }
                    //   });
                }

                return model ?? yeast;
            }
            catch (Exception e)
            {
                // await _logger.ErrorAsync(e, "Communication.TransactionAgent.UpdateAsync(TransactionsModel dto)");
                throw;
            }
        }



        public async Task<YeastPair> AddYeastPair(YeastPair yeast)
        {
            try
            {
                YeastPair model = new();

                var httpContent = new StringContent(JsonConvert.SerializeObject(yeast), Encoding.UTF8, "application/json");
                var responseMessage = await _httpClient.PostAsync($"api/yeasts/pairs", httpContent).ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var obj = JsonConvert.DeserializeObject<YeastPair>(jsonString, jsonSerializerSettings);
                    if (obj != null)
                        model = obj;
                }
                else
                {
                    // TODO handle error
                    var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    await Task.Delay(10);

                    //var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    //await _logger.WarnAsync("Unsuccessful ResponseMessage", "TransactionAgent.AddAsync(TransactionsModel dtoList) - Unsuccessful",
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
                // await _logger.ErrorAsync(e, "Communication.TransactionAgent.AddAsync(TransactionsModel dtoList)");
                throw;
            }
        }

        public async Task<bool> DeleteYeastPair(int id)
        {
            try
            {
                var responseMessage = await _httpClient.DeleteAsync($"api/yeasts/pairs/{id}").ConfigureAwait(false);
                return responseMessage.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                //await _logger.ErrorAsync(e, "Communication.TransactionAgent.DeleteAsync(int id)");
                throw;
            }
        }

        public async Task<IEnumerable<YeastPair>> GetYeastPairs()
        {
            try
            {
                List<YeastPair> model = new();

                var responseMessage = await _httpClient.GetAsync("api/yeasts/pairs").ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var list = JsonConvert.DeserializeObject<List<YeastPair>>(jsonString, jsonSerializerSettings);
                    if (list != null)
                        model = list;
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

        public async Task<YeastPair> GetYeastPair(int id)
        {
            try
            {
                YeastPair model = new();

                var responseMessage = await _httpClient.GetAsync($"api/Yeasts/pairs/{id}").ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var obj = JsonConvert.DeserializeObject<YeastPair>(jsonString, jsonSerializerSettings);
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

        public async Task<YeastPair> UpdateYeastPair(YeastPair pair)
        {
            try
            {
                YeastPair? model = null;

                var httpContent = new StringContent(JsonConvert.SerializeObject(pair), Encoding.UTF8, "application/json");
                var responseMessage = await _httpClient.PutAsync($"api/yeasts/pairs/{pair.Id}", httpContent).ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var obj = JsonConvert.DeserializeObject<YeastPair>(jsonString, jsonSerializerSettings);
                    if (obj != null)
                        model = obj;
                }
                else
                {
                    // TODO handle error
                    var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    await Task.Delay(10);

                    //var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    //await _logger.WarnAsync("Unsuccessful ResponseMessage", "TransactionAgent.UpdateAsync(TransactionsModel dto) - Unsuccessful",
                    //   new Dictionary<string, object>
                    //   {
                    // { "RequestMessage", responseMessage.RequestMessage.ToString() },
                    // { "StatusCode",responseMessage.StatusCode },
                    // { "ReasonPhrase",responseMessage.ReasonPhrase },
                    // { "Content",responseString }
                    //   });
                }

                return model ?? pair;
            }
            catch (Exception e)
            {
                // await _logger.ErrorAsync(e, "Communication.TransactionAgent.UpdateAsync(TransactionsModel dto)");
                throw;
            }
        }



        public async Task<IEnumerable<Code>> GetBrands()
        {
            try
            {
                List<Code> model = new();

                var responseMessage = await _httpClient.GetAsync("api/yeasts/brands").ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var list = JsonConvert.DeserializeObject<List<Code>>(jsonString, jsonSerializerSettings);
                    if (list != null)
                        model = list;
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

        public async Task<IEnumerable<Code>> GetBrands(int start, int length)
        {
            try
            {
                // TODO Delete if not used
                Debug.Assert(false);

                List<Code> model = new();

                var responseMessage = await _httpClient.GetAsync("api/yeasts/brands").ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var list = JsonConvert.DeserializeObject<List<Code>>(jsonString, jsonSerializerSettings);
                    if (list != null)
                        model = list;
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

        public async Task<Code> GetBrand(int id)
        {
            try
            {
                // TODO Delete if not used
                Debug.Assert(false);

                Code model = new();

                var responseMessage = await _httpClient.GetAsync($"api/MaloCultures/{id}").ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var obj = JsonConvert.DeserializeObject<Code>(jsonString, jsonSerializerSettings);
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

        public async Task<Code> AddBrand(Code brand)
        {
            try
            {
                // TODO Delete if not used
                Debug.Assert(false);

                Code model = new();

                var httpContent = new StringContent(JsonConvert.SerializeObject(brand), Encoding.UTF8, "application/json");
                var responseMessage = await _httpClient.PostAsync($"api/MaloCultures", httpContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var obj = JsonConvert.DeserializeObject<Code>(jsonString, jsonSerializerSettings);
                    if (obj != null)
                        model = obj;
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
                return model;
            }
            catch (Exception e)
            {
                // TODO await _logger.ErrorAsync(e, "Communication.TransactionAgent.AddAsync(TransactionsModel dtoList)");
                throw;
            }
        }

        public async Task<Code> UpdateBrand(Code brand)
        {
            try
            {
                // TODO Delete if not used
                Debug.Assert(false);

                Code model = new();

                var httpContent = new StringContent(JsonConvert.SerializeObject(brand), Encoding.UTF8, "application/json");
                var responseMessage = await _httpClient.PutAsync($"api/MaloCultures/{brand.Id}", httpContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var obj = JsonConvert.DeserializeObject<Code>(jsonString, jsonSerializerSettings);
                    if (obj != null)
                        model = obj;
                }
                else
                {
                    // TODO add logging
                    var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    await Task.Delay(10);

                    //await _logger.WarnAsync("Unsuccessful ResponseMessage", "TransactionAgent.UpdateAsync(TransactionsModel dto) - Unsuccessful",
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
                // TODO await _logger.ErrorAsync(e, "Communication.TransactionAgent.UpdateAsync(TransactionsModel dto)");
                throw;
            }
        }

        public async Task<bool> DeleteBrand(int id)
        {
            try
            {
                // TODO Delete if not used
                Debug.Assert(false);

                var responseMessage = await _httpClient.DeleteAsync($"api/MaloCultures/{id}");
                return responseMessage.IsSuccessStatusCode;
            }
            catch (Exception e)
            {
                // TODO await _logger.ErrorAsync(e, "Communication.TransactionAgent.DeleteAsync(int id)");
                throw;
            }

        }


        public async Task<IEnumerable<Code>> GetStyles()
        {
            try
            {

                List<Code> model = new();

                var responseMessage = await _httpClient.GetAsync("api/yeasts/styles").ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var list = JsonConvert.DeserializeObject<List<Code>>(jsonString, jsonSerializerSettings);
                    if (list != null)
                        model = list;
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

        public async Task<IEnumerable<Code>> GetStyles(int start, int length)
        {
            try
            {
                // TODO Delete if not used
                Debug.Assert(false);

                List<Code> model = new();

                var responseMessage = await _httpClient.GetAsync("api/yeasts/styles").ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var list = JsonConvert.DeserializeObject<List<Code>>(jsonString, jsonSerializerSettings);
                    if (list != null)
                        model = list;
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

        public async Task<Code> GetStyle(int id)
        {
            try
            {
                // TODO Delete if not used
                Debug.Assert(false);

                Code model = new();

                var responseMessage = await _httpClient.GetAsync($"api/MaloCultures/{id}").ConfigureAwait(false);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var obj = JsonConvert.DeserializeObject<Code>(jsonString, jsonSerializerSettings);
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

        public async Task<Code> AddStyle(Code style)
        {
            try
            {
                // TODO Delete if not used
                Debug.Assert(false);

                Code model = new();

                var httpContent = new StringContent(JsonConvert.SerializeObject(style), Encoding.UTF8, "application/json");
                var responseMessage = await _httpClient.PostAsync($"api/MaloCultures", httpContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var obj = JsonConvert.DeserializeObject<Code>(jsonString, jsonSerializerSettings);
                    if (obj != null)
                        model = obj;
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
                return model;
            }
            catch (Exception e)
            {
                // TODO await _logger.ErrorAsync(e, "Communication.TransactionAgent.AddAsync(TransactionsModel dtoList)");
                throw;
            }
        }

        public async Task<Code> UpdateStyle(Code style)
        {
            try
            {
                // TODO Delete if not used
                Debug.Assert(false);

                Code model = new();

                var httpContent = new StringContent(JsonConvert.SerializeObject(style), Encoding.UTF8, "application/json");
                var responseMessage = await _httpClient.PutAsync($"api/MaloCultures/{style.Id}", httpContent);

                if (responseMessage.IsSuccessStatusCode)
                {
                    var jsonString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var jsonSerializerSettings = new JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Auto };
                    var obj = JsonConvert.DeserializeObject<Code>(jsonString, jsonSerializerSettings);
                    if (obj != null)
                        model = obj;
                }
                else
                {
                    // TODO add logging
                    var responseString = await responseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
                    await Task.Delay(10);

                    //await _logger.WarnAsync("Unsuccessful ResponseMessage", "TransactionAgent.UpdateAsync(TransactionsModel dto) - Unsuccessful",
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
                // TODO await _logger.ErrorAsync(e, "Communication.TransactionAgent.UpdateAsync(TransactionsModel dto)");
                throw;
            }
        }

        public async Task<bool> DeleteStyle(int id)
        {
            try
            {
                // TODO Delete if not used
                Debug.Assert(false);

                var responseMessage = await _httpClient.DeleteAsync($"api/MaloCultures/{id}");
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