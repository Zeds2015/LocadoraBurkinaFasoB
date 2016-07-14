using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LocadoraBurkinaFasoWeb.Models
{
    public class Cliente
    {

        //Expressões Regulares (Validações de campo por Regex, o JS fara o trabalho no lado do Cliente.)

        [Required]
        [DisplayName("Nome")]
        [RegularExpression(@"^([a-zA-Z_\s]+)$", ErrorMessage = "Nome Inválido")]
        public string Nome { get; set; }

        [Required]
        [DisplayName("Endereço")]
        [RegularExpression(@"^(RUA|Rua|R.|AVENIDA|Avenida|AV.|TRAVESSA|Travessa|TRAV.|Trav.) ([a-zA-Z_\s]+), ([0-9]{0,})", ErrorMessage = "Modelo de Endereço: Rua Batata da Neve , 100")]
        public string Endereco { get; set; }

        /*Sem Expressões Regulares Quando o User mandar a requesição
         pro servidor, ele mesmo fara as validações. OK*/

        [Required]
        [Phone(ErrorMessage = "Digite um número de Telefone Válido")]
        public string Telefone { get; set; }


        [Required(ErrorMessage = "Digite sua Data de Nascimento")]
        [DisplayName("Data de Nascimento")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public DateTime? Data_de_Nascimento { get; set; }


        [Required(ErrorMessage = "CPF é obrigatório")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "Selecione o seu gênero")]
        public string Sexo { get; set; }
    }
}
