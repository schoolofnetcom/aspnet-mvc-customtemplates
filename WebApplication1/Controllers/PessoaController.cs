using Service.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PessoaController : Controller
    {
        //
        // GET: /Pessoa/
        public ActionResult Index()
        {
            ViewBag.Mensagem = "Minha primeira View";
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PessoaViewModel model, FormCollection formCollection)
        {
            ModelState.Remove("Codigo");

            var dia = Convert.ToInt32(formCollection["DataNascimento.days"]);
            var mes = Convert.ToInt32(formCollection["DataNascimento.months"]);
            var ano = Convert.ToInt32(formCollection["DataNascimento.years"]);

            DateTime data = new DateTime(ano, mes, dia);
            model.DataNascimento = data;

            ModelState["DataNascimento"].Errors.Clear();

            if (ModelState.IsValid)
            {
                if (model.Captcha == "123#$")
                {
                    var p = AutoMapper.Mapper.Map<PessoaViewModel, Pessoa>(model);

                    PessoaService srv = new PessoaService();
                    srv.Salvar(p);

                    return View("List", srv.Listar());
                }
                return View(model);
            }
            else
                return View(model);
        }

        public ActionResult List()
        {
            PessoaService srv = new PessoaService();
            return View(srv.Listar());
        }

        public ActionResult Edit(int id)
        {
            var srv = new PessoaService();
            var p = srv.Obter(id);

            var pVM = AutoMapper.Mapper.Map<Pessoa, PessoaViewModel>(p);

            if((pVM.Codigo % 2) == 0)
                pVM.Ativo = true;

            return View("Create", pVM);
        }

        [HttpPost]
        public ActionResult Edit(PessoaViewModel model)
        {
            if (ModelState.IsValid)
            {
                var p = AutoMapper.Mapper.Map<PessoaViewModel, Pessoa>(model);

                PessoaService srv = new PessoaService();
                srv.Salvar(p);

                return View("List", srv.Listar());
            }
            else
                return View("Create", model);
        }

        public ActionResult Delete(int id)
        {
            PessoaService srv = new PessoaService();
            srv.Deletar(id);

            return View("List", srv.Listar());
        }
	}
}