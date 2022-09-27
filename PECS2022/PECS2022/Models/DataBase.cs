using PECS2022.Models;
using PECS2022.Interfaces;
using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Forms;
using System.Threading.Tasks;
using PECS2022.SurveyModel;

namespace PECS2022
{

    public static class DataBase
    {

        private static SQLiteAsyncConnection CurrentAsyncConnection { get; set; }
        public static async Task<SQLiteAsyncConnection> GetAsyncConnection()
        {
            if (CurrentAsyncConnection == null)
            {

                var settings = DependencyService.Get<IDatabaseSettings>();
                var db = new SQLiteAsyncConnection(settings.DatabasePath);
                await db.CreateTableAsync<User>();
                await db.CreateTableAsync<Governorate>();
                await db.CreateTableAsync<Locality>();
                await db.CreateTableAsync<Profession>();
                await db.CreateTableAsync<EconomicGroup>();

                await db.CreateTableAsync<EconomicActivity>();
                await db.CreateTableAsync<Individual>();

                await db.CreateTableAsync<Visit>();
                await db.CreateTableAsync<VisitLog>();

                await db.CreateTableAsync<AddressChangeRequest>();

                await db.CreateTableAsync<SectionStatus>();
                await db.CreateTableAsync<Building>();

                await db.CreateTableAsync<ScientificSpecialization>();
                await db.CreateTableAsync<LookupVal>();
                await db.CreateTableAsync<CountryInfo>();
                await db.CreateTableAsync<QuestionComment>();

                await db.CreateTableAsync<SampleInfo>();
                 await db.CreateTableAsync<CallLogInfo>();

                CurrentAsyncConnection = db;

            }

            return CurrentAsyncConnection;
        }

        private static SQLiteConnection CurrentConnection { get; set; }
        public static SQLiteConnection GetConnection()
        {

            if (CurrentConnection == null)
            {
                var settings = DependencyService.Get<IDatabaseSettings>();
                var db = new SQLiteConnection(settings.DatabasePath);
                db.CreateTable<User>();
                db.CreateTable<Governorate>();
                db.CreateTable<Locality>();
                db.CreateTable<Profession>();
                db.CreateTable<EconomicGroup>();

                db.CreateTable<EconomicActivity>();
                db.CreateTable<Individual>();
                db.CreateTable<Visit>();
                db.CreateTable<VisitLog>();
                db.CreateTable<Building>();
                db.CreateTable<AddressChangeRequest>();
                db.CreateTable<SectionStatus>();
                db.CreateTable<ScientificSpecialization>();
                db.CreateTable<LookupVal>();
                db.CreateTable<CountryInfo>();
                db.CreateTable<QuestionComment>();

                db.CreateTable<SampleInfo>();
                db.CreateTable<CallLogInfo>();


                CurrentConnection = db;

            }

            return CurrentConnection;
        }

        public static void CloseCurrentConnection()
        {
            CurrentAsyncConnection = null;
        }

    }
}
