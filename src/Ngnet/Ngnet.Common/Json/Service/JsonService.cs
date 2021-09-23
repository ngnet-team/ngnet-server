using Newtonsoft.Json;
using System;
using System.IO;

namespace Ngnet.Common.Json.Service
{
    public class JsonService : IJsonService
    {
        public T Deserialiaze<T>(string fileName)
        {
            try
            {
                var jsonFile = File.ReadAllText(Paths.JsonDirectory + fileName);
                return JsonConvert.DeserializeObject<T>(jsonFile);
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public object Serialiaze<T>(string fileName, T model)
        {
            try
            {
                return JsonConvert.SerializeObject(model);
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
