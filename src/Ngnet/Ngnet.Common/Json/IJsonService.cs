﻿namespace Ngnet.Common.Json
{
    public interface IJsonService
    {
        public object Serialiaze<T>(string fileName, T model);

        public T Deserialiaze<T>(string fileName);
    }
}
