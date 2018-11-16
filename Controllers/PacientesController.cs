using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace NutriYA.Controllers
{
    public class PacientesController : Controller
    {   
        //int Hola = 3;
        PacientesRepository repo = new PacientesRepository();
        //public PacientesController(PacientesRepository pacientesRepo){
        //    repo = pacientesRepo;
        //}
        // GET: Pacientes
        public ActionResult Index()
        {
            List<Paciente> model = repo.LeerPaciente();
            System.Threading.Thread.Sleep(1000);
            return View(model);
        }

        // GET: Pacientes/Details/5
        public ActionResult Details(string PK, string RK)
        {
            System.Threading.Thread.Sleep(1000);
            var model = repo.LeerPorPKRK(PK,RK);
            if(model == null){
                return NotFound();
            }
            return View(model);
        }

        // GET: Pacientes/Create
        public ActionResult Create()
        {
            var model = new Paciente();
            return View(model);
        }

        // POST: Pacientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Paciente model)
        {
            try
            {
                var resultado = repo.CrearPaciente(model);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Pacientes/Edit/5
        public ActionResult Edit(string PK, string RK)
        {   
            var model = repo.LeerPorPKRK(PK, RK);
            if(model == null){
                return NotFound();
            }

            return View(model);
        }

        // POST: Pacientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Paciente model)
        {   


            if(model == null){
                return NotFound();
            }

            var pack = repo.LeerPorPKRK(model.NombreNut, model.NombrePac);

            try
            {
                pack.NombreNut = model.NombreNut;
                pack.NombrePac = model.NombrePac;
                pack.Correo = model.Correo;
                pack.Edad = model.Edad; 
                pack.Altura = model.Altura;
                pack.Peso = model.Peso;

                var resultado = repo.ActualizarPaciente(pack);
                System.Threading.Thread.Sleep(800);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Pacientes/Delete/5
        public ActionResult Delete(string PK, string RK)
        {   
            var model = repo.LeerPorPKRK(PK, RK);

            if(model == null){
                return NotFound();
            }

            return View(model);
        }

        // POST: Pacientes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Paciente model, IFormCollection collection)
        {
            if(model == null){
                return NotFound();
            }

            var pack = repo.LeerPorPKRK(model.NombreNut, model.NombrePac);

            try
            {
                System.Threading.Thread.Sleep(1000);
                var resultado = repo.BorrarPaciente(model);
                System.Threading.Thread.Sleep(1000);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}