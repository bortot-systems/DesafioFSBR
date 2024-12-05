using System.ComponentModel.DataAnnotations;

namespace DesafioFSBR.Models
{
    public class Processo
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O nome do processo é obrigatório.")]
        [Display(Name = "Nome do Processo")]
        public string NomeProcesso { get; set; }

        [Required(ErrorMessage = "O NPU é obrigatório.")]
        [RegularExpression(@"^\d{7}-\d{2}\.\d{4}\.\d\.\d{2}\.\d{4}$",
            ErrorMessage = "Formato inválido para NPU")]
        public string NPU { get; set; }

        [Required(ErrorMessage = "A data de cadastro é obrigatória.")]
        [Display(Name = "Data de Cadastro")]
        [DataType(DataType.Date)]
        public DateTime DataCadastro { get; set; }

        [Display(Name = "Data de Visualização")]
        [DataType(DataType.DateTime)]
        public DateTime? DataVisualizacao { get; set; }

        public string? DataVisualizacaoFormatada => DataVisualizacao?.ToString("dd/MM/yyyy HH:mm:ss");

        [Required(ErrorMessage = "A UF é obrigatória.")]
        public string UF { get; set; }

        [Required(ErrorMessage = "O município é obrigatório.")]
        public string Municipio { get; set; }
    }
}
