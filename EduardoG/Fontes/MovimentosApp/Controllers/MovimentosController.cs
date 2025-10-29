using Microsoft.AspNetCore.Mvc;
using MovimentosApp.Data;
using MovimentosApp.Models;
using System;

namespace MovimentosApp.Controllers
{
    public class MovimentosController : Controller
    {
        private readonly DapperMovimentoRepository _repo;
        public MovimentosController(DapperMovimentoRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index(int? mes, int? ano)
        {
            var vm = new MovimentoViewModel();
            vm.Mes = mes ?? DateTime.Now.Month;
            vm.Ano = ano ?? DateTime.Now.Year;
            vm.Produtos = _repo.GetAllProdutos();
            vm.ProximoNumeroLancamento = _repo.GetNextNumeroLancamento(vm.Mes, vm.Ano);
            vm.Cosifs = new ProdutoCosif[0];
            return View(vm);
        }

        [HttpGet]
        public IActionResult GetCosifs(string codProduto)
        {
            if (string.IsNullOrEmpty(codProduto)) return Json(new object[0]);
            var cos = _repo.GetCosifsByProduto(codProduto);
            // retornar COD_COSIF e COD_CLASSIFICACAO para exibição
            return Json(cos);
        }

        [HttpGet]
        public IActionResult GetGrid(int mes, int ano)
        {
            var res = _repo.GetMovimentosPorMesAno(mes, ano);
            return Json(res);
        }

        [HttpPost]
        public IActionResult Incluir([FromForm] MovimentoManual model)
        {
            // validações mínimas
            if(model == null) return BadRequest();
            model.DAT_MOVIMENTO = DateTime.Now;
            if (model.NUM_LANCAMENTO <= 0)
                model.NUM_LANCAMENTO = _repo.GetNextNumeroLancamento(model.DAT_MES, model.DAT_ANO);

            // pode adicionar validação se produto/cosif existem
            _repo.InsertMovimento(model);
            return RedirectToAction("Index", new { mes = model.DAT_MES, ano = model.DAT_ANO });
        }
    }
}
