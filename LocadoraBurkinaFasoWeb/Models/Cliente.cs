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

        [Required]
        [DisplayName("Nome")]
        [RegularExpression(@"^([a-zA-Z_\s]+)$", ErrorMessage = "Nome Inválido")]
        public string Nome { get; set; }

        [Required]
        [DisplayName("Endereço")]
        [RegularExpression(@"^(RUA|Rua|R.|AVENIDA|Avenida|AV.|TRAVESSA|Travessa|TRAV.|Trav.) ([a-zA-Z_\s]+), ([0-9]{0,})", ErrorMessage = "Modelo de Endereço: Rua Batata da Neve , 100")]
        public string Endereco { get; set; }

        [Required]
        [RegularExpression(@"^(([0-9]{1})*[- .(]*([0-9a-zA-Z]{3})*[- .)]*[0-9a-zA-Z]{3}[- .]*[0-9a-zA-Z]{4})+$|^\(?\d{2}\)?[\s-]?\d{4}-?\d{4}$", ErrorMessage = "Número Inválido.")]
        public string Telefone { get; set; }

        [Required(ErrorMessage = "CPF é obrigatório")]
        [RegularExpression(@"^\d*[0-9]{3}(\.\d*[0-9]{3}\.\d*[0-9]{3}\-\d*[0-9]{2})?$", ErrorMessage = "CPF Inválido")]
        [StringLength(14,MinimumLength =11,ErrorMessage ="O campo CPF tem no mínimo 11 digitos ")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "Digite sua Data de Nascimento")]
        [DisplayName("Data de Nascimento")]
        [DisplayFormat(ApplyFormatInEditMode = true)]
        public DateTime? Data_de_Nascimento { get; set; }

        [Required(ErrorMessage = "Selecione o seu gênero")]
        public string Sexo { get; set; }

    }
}
