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

        public string TotalHE050 { get; set; }

        public string TotalHE070 { get; set; }

        public string TotalHE100 { get; set; }

        public string TotalAdicionalNoturno { get; set; }

        public string TotalAtestado { get; set; }

        public string TotalPaternidade { get; set; }

        public string TotalSeguro { get; set; }

        public string TotalFaltas { get; set; }

        public string TotalFaltasJustificadas { get; set; }

        public string TotalAtrasos { get; set; }

        public string TotalCreditoBH { get; set; }

        public string TotalDebitoBH { get; set; }

        public string TotalSaldoBH { get; set; }

        public string TotalDispensaNaoRemunerada { get; set; }

        public string TotalGratAdFech { get; set; }
    }

    public class EspelhoPontoMarcacaoResult
    {
        public DateTime Data { get; set; }

        public string Marcacao { get; set; }

        public string HorasNormais { get; set; }

        public string HorasFaltas { get; set; }

        public string HE050 { get; set; }

        public string HE070 { get; set; }

        public string HE100 { get; set; }

        public string CreditoBH { get; set; }

        public string DebitoBH { get; set; }
    }
}