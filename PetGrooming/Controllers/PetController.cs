using System;
using System.Collections.Generic;
using System.Data;
//required for SqlParameter class
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetGrooming.Data;
using PetGrooming.Models;
using System.Diagnostics;
using PetGrooming.Models.ViewModel;

namespace PetGrooming.Controllers
{
    public class PetController : Controller
    {
        /*
        These reading resources will help you understand and navigate the MVC environment
 
        Q: What is an MVC controller?

        - https://docs.microsoft.com/en-us/aspnet/mvc/overview/older-versions-1/controllers-and-routing/aspnet-mvc-controllers-overview-cs

        Q: What does it mean to "Pass Data" from the Controller to the View?

        - http://www.webdevelopmenthelp.net/2014/06/using-model-pass-data-asp-net-mvc.html

        Q: What is an SQL injection attack?

        - https://www.w3schools.com/sql/sql_injection.asp

        Q: How can we prevent SQL injection attacks?

        - https://www.completecsharptutorial.com/ado-net/insert-records-using-simple-and-parameterized-query-c-sql.php

        Q: How can I run an SQL query against a database inside a controller file?

        - https://www.entityframeworktutorial.net/EntityFramework4.3/raw-sql-query-in-entity-framework.aspx
 
         */
        private PetGroomingContext db = new PetGroomingContext();
       // private UpdatePetViewModel up = new UpdatePetViewModel();

        // GET: Pet
        public ActionResult List()
        {
            //How could we modify this to include a search bar?
            var pets = db.Pets.SqlQuery("Select * from Pets").ToList();
            return View(pets);
        }
        [HttpPost]
        public ActionResult Search(string PetName)
        {
            Pet pet = db.Pets.SqlQuery("select * from Pets where PetName LIKE '% " + PetName + "%'").FirstOrDefault();
            // db.Database.ExecuteSqlCommand(query);
            return View(pet);

        }

        // GET: Pet/Details/5
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Pet pet = db.Pets.Find(id); //EF 6 technique
            Pet pet = db.Pets.SqlQuery("select * from pets where petid=@PetID", new SqlParameter("@PetID",id)).FirstOrDefault();
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        //THE [HttpPost] Means that this method will only be activated on a POST form submit to the following URL
        //URL: /Pet/Add
        [HttpPost]
        public ActionResult Add(string PetName, Double PetWeight, String PetColor, int SpeciesID, string PetNotes)
        {
            //STEP 1: PULL DATA! The data is access as arguments to the method. Make sure the datatype is correct!
            //The variable name  MUST match the name attribute described in Views/Pet/Add.cshtml

            //Tests are very useul to determining if you are pulling data correctly!
            //Debug.WriteLine("Want to create a pet with name " + PetName + " and weight " + PetWeight.ToString()) ;

            //STEP 2: FORMAT QUERY! the query will look something like "insert into () values ()"...
            string query = "insert into pets (PetName, Weight, color, SpeciesID, Notes) values (@PetName,@PetWeight,@PetColor,@SpeciesID,@PetNotes)";
            SqlParameter[] sqlparams = new SqlParameter[5]; //0,1,2,3,4 pieces of information to add
            //each piece of information is a key and value pair
            sqlparams[0] = new SqlParameter("@PetName",PetName);
            sqlparams[1] = new SqlParameter("@PetWeight", PetWeight);
            sqlparams[2] = new SqlParameter("@PetColor", PetColor);
            sqlparams[3] = new SqlParameter("@SpeciesID", SpeciesID);
            sqlparams[4] = new SqlParameter("@PetNotes",PetNotes);

            //db.Database.ExecuteSqlCommand will run insert, update, delete statements
            //db.Pets.SqlCommand will run a select statement, for example.
            db.Database.ExecuteSqlCommand(query, sqlparams);

            
            //run the list method to return to a list of pets so we can see our new one!
            return RedirectToAction("List");
            //return RedirectToAction("Pet/List");
            //both have the same thing 
        }


        public ActionResult Add()
        {
            //STEP 1: PUSH DATA!
            //What data does the Add.cshtml page need to display the interface?
            //A list of species to choose for a pet

            //alternative way of writing SQL -- will learn more about this week 4
            //List<Species> Species = db.Species.ToList();

            List<Species> species = db.Species.SqlQuery("select * from Species").ToList();

            return View(species);
        }

        //TODO:
        //Update
        public ActionResult Update(int id)
        {
            Pet Selectedpet = db.Pets.SqlQuery("select * from pets where petid =@id ", new SqlParameter("@id", id)).FirstOrDefault();
            //List<Species> species = db.Species.SqlQuery("select * from species").ToList();
            //UpdatePetViewModel viewModel = new UpdatePetViewModel();
           // viewModel.pet = Selectedpet;
            //viewModel.species =species;

            return View(Selectedpet);
        }
        //[HttpPost] Update
        [HttpPost]
        public ActionResult Update(int id,string PetName, Double PetWeight, String PetColor, string PetNotes)
        {
            string query = "update Pets SET PetName = @PetName, Weight = @PetWeight ,color = @PetColor, Notes = @PetNotes where PetID = "+id;
            SqlParameter[] sqlparams = new SqlParameter[4];
            sqlparams[0] = new SqlParameter("@PetName", PetName);
            sqlparams[1] = new SqlParameter("@PetWeight", PetWeight);
            sqlparams[2] = new SqlParameter("@PetColor", PetColor);
           // sqlparams[3] = new SqlParameter("@Speciesid", SpeciesId);
            sqlparams[3] = new SqlParameter("@PetNotes", PetNotes);

            
            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");

        }
        //[HttpPost] Delete
        //[HttpPost]
        public ActionResult Delete(int id)
        {
            string query = "Delete from Pets where PetID = @id";
            //here we  are writing pet as pets because of its db name otherwise it will give the error of invalid object name;
            //string query = "Delete From Pet where PetId = @PetId";
            SqlParameter sqlparam = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, sqlparam);
            //db.Database.ExecuteSqlCommand(query);
            return RedirectToAction("List");
        }
        //(optional) Delete
        

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
