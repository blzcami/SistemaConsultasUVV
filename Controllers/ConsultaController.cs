using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaConsultasUVV.Data;
using SistemaConsultasUVV.Models;
using System.Security.Claims;

namespace SistemaConsultasUVV.Controllers
{
    [Authorize]
    public class ConsultaController : Controller
    {
        private readonly AppDbContext _context;

        public ConsultaController(AppDbContext context)
        {
            _context = context;
        }

        // Lista somente as consultas do usuário logado
        public IActionResult Index()
        {
            int usuarioId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var consultas = _context.Consultas
                .Where(c => c.UsuarioId == usuarioId)
                .OrderBy(c => c.DataHora)
                .ToList();

            return View(consultas);
        }

        // Exibe formulário de nova consulta
        public IActionResult Criar()
        {
            return View();
        }

        // Salva nova consulta
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Criar(Consulta consulta)
        {
            if (!ModelState.IsValid)
            {
                return View(consulta);
            }

            int usuarioId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            consulta.UsuarioId = usuarioId;

            _context.Consultas.Add(consulta);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // Exibe formulário de edição
        public IActionResult Editar(int id)
        {
            int usuarioId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var consulta = _context.Consultas
                .FirstOrDefault(c =>
                    c.Id == id &&
                    c.UsuarioId == usuarioId);

            if (consulta == null)
            {
                return NotFound();
            }

            return View(consulta);
        }

        // Salva edição
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Editar(int id, Consulta consulta)
        {
            if (id != consulta.Id)
            {
                return NotFound();
            }

            int usuarioId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var consultaBanco = _context.Consultas
                .FirstOrDefault(c =>
                    c.Id == id &&
                    c.UsuarioId == usuarioId);

            if (consultaBanco == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(consulta);
            }

            consultaBanco.Especialidade = consulta.Especialidade;
            consultaBanco.DataHora = consulta.DataHora;
            consultaBanco.Descricao = consulta.Descricao;

            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // Exibe confirmação de exclusão
        public IActionResult Excluir(int id)
        {
            int usuarioId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var consulta = _context.Consultas
                .FirstOrDefault(c =>
                    c.Id == id &&
                    c.UsuarioId == usuarioId);

            if (consulta == null)
            {
                return NotFound();
            }

            return View(consulta);
        }

        // Confirma exclusão
        [HttpPost, ActionName("Excluir")]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmarExclusao(int id)
        {
            int usuarioId = int.Parse(
                User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var consulta = _context.Consultas
                .FirstOrDefault(c =>
                    c.Id == id &&
                    c.UsuarioId == usuarioId);

            if (consulta == null)
            {
                return NotFound();
            }

            _context.Consultas.Remove(consulta);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}