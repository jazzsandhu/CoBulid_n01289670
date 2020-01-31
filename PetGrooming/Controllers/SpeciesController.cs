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

namespace PetGrooming.Controllers
{
    public class SpeciesController : Controller
    {
        private PetGroomingContext db = new PetGroomingContext();
        // GET: Species
        public ActionResult Index()
        {
            return View();
        }

        //TODO: Each line should be a separate method in this class
        // List
        public ActionResult List()
        {
            //what data do we need?
            List<Species> species = db.Species.SqlQuery("select * from Species").ToList();
            return View(species);
        }
        // Show
        public ActionResult Show(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            // Pet pet = db.Pets.Find(id); //EF 6 technique
            Species species = db.Species.SqlQuery("select * from Species where Speciesid=@SpeciesId", new SqlParameter("@SpeciesId", id)).FirstOrDefault();
            if (species == null)
            {
                return HttpNotFound();
            }
            return View(species);
        }
        //fucniton for search 
        public ActionResult Search(string SpeciesName)
        {
            Species species =db.Species.SqlQuery("select * from Species where SpeciesName LIKE '% "+SpeciesName+"%'").FirstOrDefault();
           // db.Database.ExecuteSqlCommand(query);
            return View(species);

        }
        // Add
        //goto view ->Species->view
        public ActionResult Add()
        {
            return View();
        }
        //url: /sepcies/add
        //httppost add
        [HttpPost]
        public ActionResult Add(string SpeciesName)
        {
            //step1: get the user input data for the sepcies
            //step:2 run the query
            string query = "insert into Species(SpeciesName) values(@SpeciesName)";
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@SpeciesName", SpeciesName);
            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");
        }
       
        // Update
        public ActionResult Update(int id)
        {
            string query = "select *from species where speciesid = @id";
            SqlParameter sqlparam = new SqlParameter("@id", id);
            Species selectedspecies = db.Species.SqlQuery(query, sqlparam).FirstOrDefault();
            return View(selectedspecies);
        }
        [HttpPost] 
        public ActionResult Update(int id, string Name)
        {
            string query = "update Species SET SpeciesName = @SpeciesName where  SpeciesID =" + id;
            SqlParameter[] sqlparams = new SqlParameter[1];
            sqlparams[0] = new SqlParameter("@SpeciesName", Name);
            //sqlparams[1] = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            return RedirectToAction("List");
        }
        // (optional) delete
        // [HttpPost] Delete
        public ActionResult Delete(int id)
        {
            string query = "delete from Species where SpeciesId=@id";
            SqlParameter[] sqlparam = new SqlParameter[1];
            sqlparam[0] = new SqlParameter("@id", id);
            db.Database.ExecuteSqlCommand(query, sqlparam);
            return RedirectToAction("List");
        }
        //public ActionResult Show(int id)
        //{
        //    //what information do we need from the databse to make show work
        //    string query = "select * from species where SpeciesId=@id";
        //    SqlParameter sqlparam = new SqlParameter("@id", id);

        //    Species selectedspecies = db.Species.SqlQuery(query, sqlparam).FirstOrDefault();
        //    return View(selectedspecies);
        //}
    }
}