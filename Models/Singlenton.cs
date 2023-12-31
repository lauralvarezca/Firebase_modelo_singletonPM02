﻿using Firebase.Database;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Linq;

namespace Firebase_modelo_singleton.Models
{
    public class Singleton
    {
        private static Singleton instance = null;
        private readonly FirebaseClient firebaseClient;

        private Singleton()
        {
            firebaseClient = new FirebaseClient("https://miniproyecto-152e3-default-rtdb.firebaseio.com/");
        }

        public static Singleton Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Singleton();
                }
                return instance;
            }
        }

        public async Task CreateData(People data)
        {
            await firebaseClient
                .Child("people")  
                .PostAsync(data);
        }

        public async Task<List<People>> ReadData()
        {
            var peopleList = await firebaseClient
                .Child("people")  
                .OnceAsync<People>();

            return peopleList.Select(item => {
                var people = item.Object;
                people.id=item.Key; // Asigna el ID de Firebase al objeto people
                return people;
            }).ToList();
        }

        public async Task UpdateData(string key, People data)
        {
            await firebaseClient
                .Child("people")  
                .Child(key)
                .PutAsync(data);
        }

        public async Task DeleteData(string key)
        {
            await firebaseClient
                .Child("people")  
                .Child(key)
                .DeleteAsync();
        }
    }
}
