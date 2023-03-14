using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using RESTFULAPI_Automation_2._1;

namespace RESTFULAPI_Automation_2._1
{
    [TestClass]
    public class HttpClientTest
    {
        private static HttpClient httpClient;

        private static readonly string BaseURL = "https://petstore.swagger.io/v2/";

        private static readonly string PetsEndpoint = "pet";
        private static string GetURL(string enpoint) => $"{BaseURL}{enpoint}";
        private static Uri GetURI(string endpoint) => new Uri(GetURL(endpoint));

        private readonly List<PetModel> cleanUpList = new List<PetModel>();

        [TestInitialize]
        public void TestInitialize()
        {
            httpClient = new HttpClient();
        }

        [TestCleanup]
        public async Task TestCleanUp()
        {
            foreach (var data in cleanUpList)
            {
                var httpResponse = await httpClient.DeleteAsync(GetURL($"{PetsEndpoint}/{data.Id}"));
            }
        }



        [TestMethod]
        public async Task PutPetMethod()
        {
            #region create data
            Category category = new Category();
            category.Id = 10;
            category.Name = "TestCategory";

            Category tag1 = new Category()
            {
                Id = 20,
                Name = "Tag1"
            };
            Category tag2 = new Category()
            {
                Id = 30,
                Name = "Tag2"
            };
            // Create Json Object
            PetModel petData = new PetModel()
            {
                Id = 10,
                PetCategory = new Category()
                {
                    Id = 40,
                    Name = "Hound"
                },

                Name = "Hunter",
                PhotoUrls = new List<string> { "url3", "url4" },
                PetTags = new List<Category> { tag1, tag2 },
                Status = "READY",
            };

            // Serialize Content
            var request = JsonConvert.SerializeObject(petData);
            var postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Post Request
            await httpClient.PostAsync(GetURL(PetsEndpoint), postRequest);

            #endregion

            #region get Username of the created data

            // Get Request
            var getResponse = await httpClient.GetAsync(GetURI($"{PetsEndpoint}/{petData.Id}"));

            // Deserialize Content
            var listPetData = JsonConvert.DeserializeObject<PetModel>(getResponse.Content.ReadAsStringAsync().Result);

            // filter created data
            var createdPetData = listPetData;

            #endregion

            #region send put request to update data

            // Update value of petData
            Category updatedCategory = new Category();
            category.Id = 10;
            category.Name = "TestCategory";

            Category updatedTag1 = new Category()
            {
                Id = 20,
                Name = "updatedTag1"
            };
            Category updatedTag2 = new Category()
            {
                Id = 30,
                Name = "updatedTag2"
            };

            petData = new PetModel()
            {
                Id = listPetData.Id,
                PetCategory = new Category()
                {
                    Id = 100,
                    Name = "Herding"
                },
                Name = "Maharlika",
                PhotoUrls = new List<string> { "url8", "url9" },
                PetTags = new List<Category> { updatedTag1, updatedTag2 },
                Status = listPetData.Status
            };


            // Serialize Content
            request = JsonConvert.SerializeObject(petData);
            postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Put Request
            var httpResponse = await httpClient.PutAsync(GetURL($"{PetsEndpoint}"), postRequest);

            // Get Status Code
            var statusCode = httpResponse.StatusCode;

            #endregion

            #region get updated data

            // Get Request
            getResponse = await httpClient.GetAsync(GetURI($"{PetsEndpoint}/{petData.Id}"));

            // Deserialize Content
            listPetData = JsonConvert.DeserializeObject<PetModel>(getResponse.Content.ReadAsStringAsync().Result);

            // filter created data
            createdPetData = listPetData;

            #endregion

            #region cleanup data

            // Add data to cleanup list
            cleanUpList.Add(listPetData);

            #endregion

            #region assertion

            // Assertion
            Assert.AreEqual(HttpStatusCode.OK, statusCode, "--Status code is not 201--");
            Assert.AreEqual(petData.Name, listPetData.Name, "Name does not match!");
            Assert.AreEqual(petData.Id, listPetData.Id, "ID doesn't match!");

            #endregion

        }

        [TestMethod]
        public async Task DeletePetMethod()
        {
            #region create data

            Category category = new Category();
            category.Id = 10;
            category.Name = "TestCategory";

            Category tag1 = new Category()
            {
                Id = 20,
                Name = "Tag1"
            };
            Category tag2 = new Category()
            {
                Id = 30,
                Name = "Tag2"
            };
            // Create Json Object
            PetModel petData = new PetModel()
            {
                Id = 10,
                PetCategory = new Category()
                {
                    Id = 40,
                    Name = "Hound"
                },

                Name = "Hunter",
                PhotoUrls = new List<string> { "url3", "url4" },
                PetTags = new List<Category> { tag1, tag2 },
                Status = "READY",
            };

            // Serialize Content
            var request = JsonConvert.SerializeObject(petData);
            var postRequest = new StringContent(request, Encoding.UTF8, "application/json");

            // Send Post Request
            await httpClient.PostAsync(GetURL(PetsEndpoint), postRequest);

            #endregion

            #region get Username of the created data

            // Get Request
            var getResponse = await httpClient.GetAsync(GetURI($"{PetsEndpoint}/{petData.Id}"));

            // Deserialize Content
            var listPetData = JsonConvert.DeserializeObject<PetModel>(getResponse.Content.ReadAsStringAsync().Result);

            // filter created data
            var createdPetDataId = listPetData.Id;

            #endregion

            #region send delete request

            // Send Delete Request
            var httpResponse = await httpClient.DeleteAsync(GetURL($"{PetsEndpoint}/{createdPetDataId}"));

            // Get Status Code
            var statusCode = httpResponse.StatusCode;

            #endregion

            #region assertion

            // Assertion
            Assert.AreEqual(HttpStatusCode.OK, statusCode, "--Status code is not 201--");

            #endregion
        }

    }
}