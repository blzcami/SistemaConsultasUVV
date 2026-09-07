using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaConsultasUVV.Data;
using SistemaConsultasUVV.Models;
using System.Security.Claims;

namespace SistemaConsultasUVV.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IPasswordHasher<Usuario> _passwordHasher;

        public UsuarioController(
            AppDbContext context,
            IPasswordHasher<Usuario> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }


        
        // CADASTRO
        

        [HttpGet]
        public IActionResult Cadastro()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cadastro(Usuario usuario)
        {
            if (!ModelState.IsValid)
            {
                return View(usuario);
            }

            // Verifica se o e-mail já está cadastrado
            if (_context.Usuarios.Any(u => u.Email == usuario.Email))
            {
                ModelState.AddModelError(
                    "Email",
                    "Este e-mail já está cadastrado.");

                return View(usuario);
            }

            // Data de cadastro
            usuario.DataCadastro = DateTime.Now;

            // Transforma a senha em hash antes de salvar
            usuario.Senha = _passwordHasher.HashPassword(
                usuario,
                usuario.Senha);

            // Salva o usuário
            _context.Usuarios.Add(usuario);
            _context.SaveChanges();

            // Depois do cadastro, vai para o login
            return RedirectToAction(nameof(Login));
        }


       
        // LOGIN
        

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(
            string email,
            string senha)
        {
            // Procura o usuário apenas pelo e-mail
            var usuario = _context.Usuarios
                .FirstOrDefault(u => u.Email == email);

            // Usuário não encontrado
            if (usuario == null)
            {
                ViewBag.Erro = "E-mail ou senha inválidos.";
                return View();
            }

            // Verifica a senha digitada contra o hash salvo
            var resultado = _passwordHasher.VerifyHashedPassword(
                usuario,
                usuario.Senha,
                senha);

            // Senha incorreta
            if (resultado == PasswordVerificationResult.Failed)
            {
                ViewBag.Erro = "E-mail ou senha inválidos.";
                return View();
            }


            
            // CRIAÇÃO DA AUTENTICAÇÃO
            

            var claims = new List<Claim>
            {
                new Claim(
                    ClaimTypes.NameIdentifier,
                    usuario.Id.ToString()),

                new Claim(
                    ClaimTypes.Name,
                    usuario.Nome),

                new Claim(
                    ClaimTypes.Email,
                    usuario.Email)
            };


            var identidade = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);


            var principal = new ClaimsPrincipal(identidade);


            // Login não persistente
            var propriedades = new AuthenticationProperties
            {
                IsPersistent = false
            };


            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                propriedades);


            // Vai para as consultas
            return RedirectToAction(
                "Index",
                "Consulta");
        }


        
        // LOGOUT
      

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(Login));
        }


     
        // ACESSO NEGADO
        

        [HttpGet]
        public IActionResult AcessoNegado()
        {
            return View();
        }
    }
}