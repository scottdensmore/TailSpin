//===============================================================================
// Microsoft patterns & practices
// Windows Phone 7 Developer Guide
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// This code released under the terms of the 
// Microsoft patterns & practices license (http://wp7guide.codeplex.com/license)
//===============================================================================


namespace TailSpin.PhoneClient.Services.Stores
{
    using System.Collections.Generic;
    using System.IO;
    using System.IO.IsolatedStorage;
    using System.Linq;
    using TailSpin.PhoneClient.Models;

    public class SurveyStore : ISurveyStore
    {
        private readonly string storeName;

        public SurveyStore(string storeName)
        {
            this.storeName = storeName;
            this.Initialize();
        }

        public SurveysList AllSurveys { get; set; }

        public string LastSyncDate
        {
            get
            {
                return this.AllSurveys.LastSyncDate;
            }

            set
            {
                this.AllSurveys.LastSyncDate = value;
                this.SaveStore();
            }
        }

        public IEnumerable<SurveyTemplate> GetSurveyTemplates()
        {
            return this.AllSurveys.Templates;
        }

        public IEnumerable<SurveyAnswer> GetCompleteSurveyAnswers()
        {
            return this.AllSurveys.Answers.Where(a => a.IsComplete);
        }

        public void SaveSurveyTemplates(IEnumerable<SurveyTemplate> surveys)
        {
            foreach (var s in surveys)
            {
                s.IsNew = true;
            }
            foreach (var s in this.AllSurveys.Templates)
            {
                s.IsNew = false;
            }
            this.AllSurveys.Templates.AddRange(surveys.Where(ns => !this.AllSurveys.Templates.Any(s => s.SlugName == ns.SlugName && s.Tenant == ns.Tenant)));
            this.SaveStore();
        }

        public void SaveStore()
        {
            lock (this)
            {
                using (var filesystem = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var fs = new IsolatedStorageFileStream(this.storeName, FileMode.Create, filesystem))
                    {
                        var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(SurveysList));
                        serializer.WriteObject(fs, this.AllSurveys);
                    }
                }
            }
        }

        public void SaveSurveyAnswer(SurveyAnswer surveyAnswer)
        {
            if (!this.AllSurveys.Answers.Contains(surveyAnswer))
            {
                this.AllSurveys.Answers.Add(surveyAnswer);
            }

            this.SaveStore();
        }

        public SurveyAnswer GetCurrentAnswerForTemplate(SurveyTemplate template)
        {
            return this.AllSurveys.Answers.Where(a => template.SlugName == a.SlugName && template.Tenant == a.Tenant && !a.IsComplete).FirstOrDefault();
        }

        public void DeleteSurveyAnswers(IEnumerable<SurveyAnswer> surveyAnswersToDelete)
        {
            var filesToDelete =
                from answers in surveyAnswersToDelete
                from answer in answers.Answers
                where
                    answer.Value != null && (answer.QuestionType == QuestionType.Voice || answer.QuestionType == QuestionType.Picture)
                select answer.Value;

            using (var filesystem = IsolatedStorageFile.GetUserStoreForApplication())
            {
                foreach (var file in filesToDelete)
                {
                    if (filesystem.FileExists(file))
                    {
                        filesystem.DeleteFile(file);
                    }
                }
            }

            surveyAnswersToDelete.ToList().ForEach(a => this.AllSurveys.Answers.Remove(a));

            this.SaveStore();
        }

        private void Initialize()
        {
            lock (this)
            {
                using (var filesystem = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (!filesystem.FileExists(this.storeName))
                    {
                        this.AllSurveys = new SurveysList { Templates = new List<SurveyTemplate>(), Answers = new List<SurveyAnswer>() };
                    }
                    else
                    {
                        using (var fs = new IsolatedStorageFileStream(this.storeName, FileMode.Open, filesystem))
                        {
                            var serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(SurveysList));
                            this.AllSurveys = serializer.ReadObject(fs) as SurveysList;
                        }
                    }
                }
            }
        }
    }
}
