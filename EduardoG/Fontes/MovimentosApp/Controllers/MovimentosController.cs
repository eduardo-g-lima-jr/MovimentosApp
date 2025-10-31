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

            // Carregar grid com registros do mês/ano atual
            vm.Movimentos = _repo.GetMovimentosPorMesAno(vm.Mes, vm.Ano);
            return View(vm);
        }

        [HttpGet]
        public IActionResult GetCosifs(string codProduto)
        {
            if (string.IsNullOrEmpty(codProduto))
                return Json(new { erro = "Produto inválido" });

            var cosifs = _repo.GetCosifsByProduto(codProduto);
            return Json(cosifs);
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
            if (model == null) return BadRequest();

            model.DAT_MOVIMENTO = DateTime.Now;
            model.NUM_LANCAMENTO = _repo.GetNextNumeroLancamento(model.DAT_MES, model.DAT_ANO);
            model.COD_USUARIO = "user01";
            
            _repo.InsertMovimento(model);
            return RedirectToAction("Index", new { mes = model.DAT_MES, ano = model.DAT_ANO });
        }
    }
}
