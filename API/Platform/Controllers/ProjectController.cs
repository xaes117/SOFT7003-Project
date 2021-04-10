﻿using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Platform.Controllers
{
    public class ProjectController : ApiController
    {
        private DataManager dataManager;

        // POST api/<controller>
        // Post a project
        public string Post(string jwt, string projectTitle, string projectDescription)
        {
            string getUidQuery =
            "select                                      " +
            "users.uid,                                  " +
            "users.acc_type                              " +
            "from users                                  " +
            "left join web_tokens t on t.uid = users.uid " +
            "where t.jwt = '" + jwt + "';";

            try
            {
                List<List<string>> data = this.dataManager.Select(getUidQuery);
                string uid = data[0][0];
                string accountType = data[0][1];

                if (accountType.ToLower().Equals("student"))
                {
                    return "students not allowed to post projects";
                }

                // Set project ID
                string maxUidQuery = "SELECT MAX(cast(projectid as unsigned)) FROM soft7003.projects;";
                int projectId = Int32.Parse(this.dataManager.Select(maxUidQuery)[0][0]) + 1;


                try
                {
                    // create insert string
                    string insertQuery = "INSERT INTO `soft7003`.`projects` (`projectid`, `owner_id`, `project_name`, `description`) " +
                    "VALUES (" +
                    "'" + projectId + "'," +
                    " '" + uid + "', " +
                    "'" + projectTitle + "', " +
                    "'" + projectDescription + "');";

                    if (Int32.Parse(uid) < 0)
                    {
                        throw new Exception("Cannot have negative uids");
                    }

                    // attempt insert
                    this.dataManager.Insert(insertQuery);

                    return "{ " +
                        "'jwt' : '" + jwt + "'," +
                        "'message' : 'Successfully created project'," +
                        "'projectId' : '" + projectId + "'" +
                        "}";

                } catch (Exception e)
                {
                    throw e;
                }

            } catch (Exception e)
            {
                return "could not find project owner";
            }
        }

        // PUT api/<controller>/5
        // Submit a proposal
        public string Put(int projectId, string jwt, string coverLetter)
        {
            string getUidQuery =
            "select                                      " +
            "users.uid                                   " +
            "from users                                  " +
            "left join web_tokens t on t.uid = users.uid " +
            "where t.jwt = '" + jwt + "';";

            try
            {
                string uid = this.dataManager.Select(getUidQuery)[0][0];

                string insertQuery = "INSERT INTO `soft7003`.`proposals` (`project_id`, `student_id`, `cover_letter`) " +
                "VALUES (" +
                "'" + projectId + "', " +
                "'" + uid + "', " +
                "'" + coverLetter + "');";

                this.dataManager.Insert(insertQuery);

                return "{ " +
                    "'message' : 'success'," +
                    "'jwt' : '" + jwt + "' }";

            } catch (Exception e)
            {
                return e.ToString();
            }

        }

        public ProjectController()
        {
            this.dataManager = new DataManager();
        }

        public ProjectController(DataManager dataManager)
        {
            this.dataManager = dataManager;
        }
    }
}