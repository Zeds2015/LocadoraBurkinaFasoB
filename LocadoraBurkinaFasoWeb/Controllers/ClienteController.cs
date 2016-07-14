using LocadoraBurkinaFasoWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace LocadoraBurkinaFasoWeb.Controllers
{
    public class ClienteController : Controller
    {

        //Primeira Requisição a Página, e Aparição do Formulário.

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

            var Telefone_valido = Validar_Telefone(cliente.Telefone);
            var CPF_valido = Valida_CPF(cliente.CPF);


            if (ModelState.IsValid && validar_data && Telefone_valido && CPF_valido)
            {
                cliente.Telefone = cliente.Telefone.Insert(0, "(");
                cliente.Telefone = cliente.Telefone.Insert(3, ")");
                cliente.Telefone = cliente.Telefone.Insert(4, " ");
                cliente.Telefone = cliente.Telefone.Insert(9, "-");

                cliente.CPF = cliente.CPF.Insert(3, ".");
                cliente.CPF = cliente.CPF.Insert(7, ".");
                cliente.CPF = cliente.CPF.Insert(11, "-");


                return View("Criado_Cliente", cliente);
            }

            return View("Criar_Cliente");
        }



        //Validação de Telefones Brasileiros

        [HttpPost]
        public bool Validar_Telefone(string Telefone_do_Cliente)
        {
            var verificador_de_numeros = 0;
            char[] vetor_de_caractere;

            try
            {
                vetor_de_caractere = Telefone_do_Cliente.ToCharArray();
            }
            catch
            {
                return false;
            }

            for (int i = 0; i < Telefone_do_Cliente.Length; i++)
            {
                if (char.IsLetter(vetor_de_caractere[i]))
                {
                    verificador_de_numeros++;
                }
            }

            if (verificador_de_numeros > 0)
            {
                ModelState.AddModelError("", "Telefone não possui letras");
                return false;
            }

            else
            {
                // Se não possuir letras
                //Retira o 55 se estiver na posição 0 e 1.

                if (Telefone_do_Cliente[0] == '5' && Telefone_do_Cliente[1] == '5')
                {
                    Telefone_do_Cliente = Telefone_do_Cliente.Remove(0, 2);
                }

                Telefone_do_Cliente = Telefone_do_Cliente.Replace("(", "");
                Telefone_do_Cliente = Telefone_do_Cliente.Replace(")", "");
                Telefone_do_Cliente = Telefone_do_Cliente.Replace(" ", "");
                Telefone_do_Cliente = Telefone_do_Cliente.Replace("-", "");

                var Telefone = Telefone_do_Cliente;
                var condicao_telefone1 = Telefone.Length == 11;
                var condicao_telefone2 = Telefone.Length == 10;


                //Se o telefone não tiver 11 (Celular) ou 10 (Fixo) Digitos ele dara erro. (21) 98617 2746

                if (!condicao_telefone1 && !condicao_telefone2)
                {
                    ModelState.AddModelError("", "Número de Telefone Inválido");
                    return false;
                }

                int[] vetor_inteiro_telefone = new int[Telefone.Length];

                for (int i = 0; i < Telefone.Length; i++)
                {
                    vetor_inteiro_telefone[i] = int.Parse(Telefone[i].ToString());
                }

                if (vetor_inteiro_telefone.Length == 11 && vetor_inteiro_telefone[2] == 9)
                {
                    var validar_Telefone_Celular = vetor_inteiro_telefone[0] >= 2;
                    return validar_Telefone_Celular;
                }

                else if (vetor_inteiro_telefone.Length == 10 && vetor_inteiro_telefone[2] != 9)
                {
                    var validar_Telefone_Fixo = vetor_inteiro_telefone[0] >= 2 && vetor_inteiro_telefone[2] >= 2;
                    return validar_Telefone_Fixo;
                }
                ModelState.AddModelError("", "Telefone Inválido");
                return false;
            }
        }

        //Validação de CPF.

        [HttpPost]
        public bool Valida_CPF(string CPF_do_Cliente)
        {
            var CPF = CPF_do_Cliente;

            try
            {
                CPF = CPF.Replace(".", "");
                CPF = CPF.Replace("-", "");
            }
            catch
            {
                return false;
            }

            var vetor_caracteres_CPF = CPF.ToCharArray();
            var verificador_de_numeros = 0;


            for (int i = 0; i < CPF.Length; i++)
            {
                if (char.IsLetter(vetor_caracteres_CPF[i]))
                {
                    verificador_de_numeros++;
                }
            }

            if (verificador_de_numeros > 0)
            {
                ModelState.AddModelError("", "CPF não possuem letras");
                return false;
            }

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

        //Se todo o Formulário estiver preenchido corretamente ele executa essa Action

        public ActionResult Criado_Cliente(Cliente cliente)
        {
            return View(cliente);
        }
    }
}