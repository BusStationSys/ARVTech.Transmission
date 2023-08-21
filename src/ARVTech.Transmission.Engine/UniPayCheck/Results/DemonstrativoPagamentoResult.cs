namespace ARVTech.Transmission.Engine.UniPayCheck.Results
{
    using System.Collections.Generic;

    public class DemonstrativoPagamentoResult
    {
        public string RazaoSocial { get; set; }

        public string Cnpj { get; set; }

        public string Competencia { get; set; }

        public string Matricula { get; set; }

        public string Cpf { get; set; }

        public string Nome { get; set; }

        public string DescricaoCargo { get; set; }

        public string DescricaoSetor { get; set; }

        public string NumeroCtps { get; set; }

        public string SerieCtps { get; set; }

        public string UfCtps { get; set; }

        public string DataAdmissao { get; set; }

        public string SalarioNominal { get; set; }

        public string Ir { get; set; }

        public string Sf { get; set; }

        public string Banco { get; set; }

        public string Agencia { get; set; }

        public string Conta { get; set; }

        public List<DemonstrativoPagamentoEventoResult> Eventos { get; set; }

        public string BaseFgts { get; set; }

        public string ValorFgts { get; set; }

        public string TotalVencimentos { get; set; }

        public string TotalDescontos { get; set; }

        public string BaseIrrf { get; set; }

        public string BaseInss { get; set; }

        public string TotalLiquido { get; set; }

        public DemonstrativoPagamentoResult()
        {
            this.Cnpj = "00000000000000";
            this.Cpf = "00000000000";
        }
    }

    public class DemonstrativoPagamentoEventoResult
    {
        public string Codigo { get; set; }

        public string Descricao { get; set; }

        public string Referencia { get; set; }

        public string Tipo { get; set; }

        public string Valor { get; set; }
    }
}