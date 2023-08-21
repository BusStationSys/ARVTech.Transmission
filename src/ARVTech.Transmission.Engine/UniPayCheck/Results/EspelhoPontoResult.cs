namespace ARVTech.Transmission.Engine.UniPayCheck.Results
{
    public class EspelhoPontoResult
    {
        public string Cnpj { get; set; }

        public string CargaHoraria { get; set; }

        public string Competencia { get; set; }

        public string DescricaoSetor { get; set; }

        public string Matricula { get; set; }

        public string Nome { get; set; }

        public string RazaoSocial { get; set; }

        public List<EspelhoPontoMarcacaoResult> Marcacoes { get; set; }
    }

    public class EspelhoPontoMarcacaoResult
    {
        public DateTime DataMarcacao { get; set; }

        public string HorarioMarcacao1 { get; set; }

        public string HorarioMarcacao2 { get; set; }

        public string HorarioMarcacao3 { get; set; }

        public string HorarioMarcacao4 { get; set; }

        public string HorarioMarcacao5 { get; set; }

        public string HorarioMarcacao6 { get; set; }

        public string HorarioMarcacao7 { get; set; }

        public string HorarioMarcacao8 { get; set; }

        public string Trabalhadas { get; set; }

        public string Faltas { get; set; }

        public string HE050 { get; set; }

        public string HE070 { get; set; }

        public string HE100 { get; set; }

        public string CreditoBH { get; set; }

        public string DebitoBH { get; set; }
    }
}