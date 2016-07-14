using LocadoraBurkinaFasoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LocadoraBurkinaFasoWeb.Controllers
{
    public class ClienteController : Controller
    {
        public ActionResult Criar_Cliente()
        {
            var cliente = new Cliente();

            return View(cliente);
        }

        // Verificação do Formulário + Validação de Data de Nascimento.

        [HttpPost]
        public ActionResult Criar_Cliente(Cliente cliente)
        {

            var data = DateTime.Now;
            var validar_data = cliente.Data_de_Nascimento < data;

            if (!validar_data)
            {
                ModelState.AddModelError("", "Por favor escolha uma data inferior a " + DateTime.Now);
            }

            var CPF_valido = Valida_CPF(cliente.CPF);

            if (ModelState.IsValid && validar_data && CPF_valido)
            {
                cliente.CPF = cliente.CPF.Insert(3, ".");
                cliente.CPF = cliente.CPF.Insert(7, ".");
                cliente.CPF = cliente.CPF.Insert(11, "-");

                return View("Index", "Home");
            }

            return View("Criar_Cliente");
        }

        //Validação de CPF.

        [HttpPost]
        public bool Valida_CPF(string CPF_do_Cliente)
        {
            var CPF = CPF_do_Cliente;

                CPF = CPF.Replace(".", "");
                CPF = CPF.Replace("-", "");

            //Verifica se o CPF tem 11 digitos.

            if (CPF.Length == 11)
            {
                var CPF_em_inteiros = new int[CPF.Length];

                for (int i = 0; i < CPF.Length; i++)
                {
                    CPF_em_inteiros[i] = int.Parse(CPF[i].ToString());
                }

                var maximo = CPF_em_inteiros.Max();
                var minimo = CPF_em_inteiros.Min();

                /* Por isso eu amo Raciocínio Lógico e Lógica Matemática.
                Resposta Todos são iguais pois não há maior nem menor.*/

                if (maximo == minimo)
                {
                    ModelState.AddModelError("", "CPF Inválido, não existe CPF com todos os números iguais");
                    return false;
                }

                var Soma = 0;

                for (int i = 0; i < 9; i++)
                {
                    Soma += CPF_em_inteiros[i] * (10 - i);
                }

                var resultado = Soma % 11;
                var digito1 = 11 - resultado;

                if (resultado < 2)
                {
                    digito1 = 0;
                }

                //Verifica se o primeiro digito verificador combina

                if (CPF_em_inteiros[9] == digito1)
                {
                    Soma = 0;

                    for (int i = 0; i < 10; i++)
                    {
                        Soma += CPF_em_inteiros[i] * (11 - i);
                    }

                    resultado = Soma % 11;
                    var digito2 = 11 - resultado;

                    if (resultado < 2)
                    {
                        digito2 = 0;
                    }

                    //Confere se o segundo digito verificador combina e assim cadastra o CPF.
                    if (CPF_em_inteiros[10] == digito2)
                    {
                        return true;
                    }

                    ModelState.AddModelError("", "CPF Inválido");
                    return false;
                }

                ModelState.AddModelError("", "CPF Inválido");
                return false;
            }

            ModelState.AddModelError("", "CPF Inválido");
            return false;
        }

        public ActionResult Criado_Cliente(Cliente cliente)
        {

            string userID = Membership.GetUser().ProviderUserKey.ToString();

            return View(cliente);
        }
    }
}