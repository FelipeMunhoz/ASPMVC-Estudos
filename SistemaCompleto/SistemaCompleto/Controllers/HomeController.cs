using SistemaCompleto.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SistemaCompleto.Controllers {

    public class HomeController : Controller {

        public ActionResult Index() {
            Session.Clear();
            return View();           
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(User user) {

            if (ModelState.IsValid) {

                using (CadastroEntities dc = new CadastroEntities()) {
                    var v = dc.Users.Where(a => a.username.Equals(user.username) && a.password.Equals(user.password)).FirstOrDefault();

                    if(v != null) {
                        Session["usuarioLogadoID"] = v.id.ToString();
                        Session["nomeUsuarioLogado"] = v.username.ToString();
                        return RedirectToAction("Login");
                    }
                }

            }
            
            return View(user);

        }

        public ActionResult Login() {

            if (Session["usuarioLogadoID"] != null)
                return View();
            else
                return RedirectToAction("Index");

        }

        public ActionResult Register() {

            if (Session["usuarioLogadoID"] == null)
                return View();
            else
                return RedirectToAction("Index");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user) {

            if (ModelState.IsValid) {

                using (CadastroEntities dc = new CadastroEntities()) {
                    var v = dc.Users.Where(a => a.username.Equals(user.username)).FirstOrDefault();

                    if (v == null) {
                        dc.Users.Add(user);
                        dc.SaveChanges();
                        return RedirectToAction("Index");
                    }
                }

            }

            return View(user);
        }

        public ActionResult ShowUsers() {

            if (Session["usuarioLogadoID"] == null)
                return RedirectToAction("Index");

            var users = new List<User>();

            using (CadastroEntities dc = new CadastroEntities()) {
                users = dc.Users.ToList();
            }

            return View(users);

        }

    }
}