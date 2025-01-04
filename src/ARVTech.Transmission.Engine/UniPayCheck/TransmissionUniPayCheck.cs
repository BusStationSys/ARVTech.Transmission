namespace ARVTech.Transmission.Engine.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text;
    using ARVTech.Transmission.Engine.UniPayCheck.Results;

    public class TransmissionUniPayCheck
    {
        private readonly bool _isDirectory = false;
        private readonly bool _isFile = false;

        private readonly string _pathDirectoryOrFileName;

        private readonly List<string> _filesDemonstrativoPagamento;
        private readonly List<string> _filesEmpregador;
        private readonly List<string> _filesEspelhoPonto;
        private readonly List<string> _filesMatricula;

        private readonly string _searchPatternDemonstrativoPagamento = "DemonstrativoPagamento*.txt";
        private readonly string _searchPatternEmpregador = "Empregador*.txt";
        private readonly string _searchPatternEspelhoPonto = "EspelhoPonto*.txt";
        private readonly string _searchPatternMatricula = "Matricula*.txt";

        /// <summary>
        /// 
        /// </summary>
        public bool IsDirectory
        {
            get
            {
                return this._isDirectory;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsFile
        {
            get
            {
                return this._isFile;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string PathDirectoryOrFileName
        {
            get
            {
                return this._pathDirectoryOrFileName;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransmissionUniPayCheck"/> class.
        /// </summary>
        /// <param name="pathDirectoryOrFileName">Path`s Directory or Filename to be processed. If it is a Directory, all files that are there will be considered. Otherwise, if it is a single file, it will only be considered.</param>
        public TransmissionUniPayCheck(string pathDirectoryOrFileName)
        {
            if (string.IsNullOrEmpty(pathDirectoryOrFileName))
            {
                throw new ArgumentNullException(
                    nameof(
                        pathDirectoryOrFileName));
            }

            this._filesDemonstrativoPagamento = new List<string>();
            this._filesEspelhoPonto = new List<string>();
            this._filesEmpregador = new List<string>();
            this._filesMatricula = new List<string>();

            if (File.Exists(pathDirectoryOrFileName))   // Is File (Individual).
            {
                this._filesEmpregador.Add(
                    pathDirectoryOrFileName);

                this._filesMatricula.Add(
                    pathDirectoryOrFileName);

                this._filesDemonstrativoPagamento.Add(
                    pathDirectoryOrFileName);

                this._filesEspelhoPonto.Add(
                    pathDirectoryOrFileName);

                this._isFile = true;
            }
            else if (Directory.Exists(pathDirectoryOrFileName)) // Is Folder (Collective).
            {
                this._filesEmpregador = Directory.EnumerateFiles(
                    pathDirectoryOrFileName,
                    this._searchPatternEmpregador,
                    SearchOption.TopDirectoryOnly).ToList();

                this._filesMatricula = Directory.EnumerateFiles(
                    pathDirectoryOrFileName,
                    this._searchPatternMatricula,
                    SearchOption.TopDirectoryOnly).ToList();

                this._filesDemonstrativoPagamento = Directory.EnumerateFiles(
                    pathDirectoryOrFileName,
                    this._searchPatternDemonstrativoPagamento,
                    SearchOption.TopDirectoryOnly).ToList();

                this._filesEspelhoPonto = Directory.EnumerateFiles(
                    pathDirectoryOrFileName,
                    this._searchPatternEspelhoPonto,
                    SearchOption.TopDirectoryOnly).ToList();

                this._isDirectory = true;
            }
            else
                throw new FileNotFoundException($@"Diretório ou Arquivo {pathDirectoryOrFileName} não encontrado.");

            this._pathDirectoryOrFileName = pathDirectoryOrFileName;
        }

        /// <summary>
        /// Collective or Individual.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EmpregadorResult> GetEmpregadores()
        {
            try
            {
                if (this._filesEmpregador is null ||
                    this._filesEmpregador.Count == 0)
                    return null;

                var empregadoresResult = new List<EmpregadorResult>();

                foreach (var fileEmpregador in this._filesEmpregador)
                {
                    var lines = File.ReadAllLines(
                        fileEmpregador);

                    foreach (var line in lines)
                    {
                        empregadoresResult.Add(
                            new EmpregadorResult()
                            {
                                Cnpj = line.Substring(0, 19).Trim(),
                                RazaoSocial = line.Substring(19, 40).Trim(),
                                DataFundacao = line.Substring(59, 10).Trim(),
                                Cep = line.Substring(69, 10).Trim(),
                                Logradouro = line.Substring(79, 40).Trim(),
                                NumeroLogradouro = line.Substring(119, 6).Trim(),
                                Complemento = line.Substring(125, 40).Trim(),
                                Bairro = line.Substring(165, 40).Trim(),
                                Cidade = line.Substring(205, 30).Trim(),
                                Uf = line.Substring(235, 2).Trim(),
                                Email = line.Substring(239, 100).Trim(),
                                Telefone = line.Substring(339, 23).Trim(),
                                UnidadeNegocio = line.Substring(362, 30).Trim(),
                            });
                    }
                }

                return empregadoresResult;
            }
            catch
            {
                throw;
            }
        }

        public string GetConteudoEmpregadores()
        {
            try
            {
                if (this._filesEmpregador is null ||
                    this._filesEmpregador.Count == 0)
                    return null;

                var conteudo = string.Empty;

                foreach (var fileEmpregador in this._filesEmpregador)
                {
                    if (!string.IsNullOrEmpty(conteudo))
                        conteudo = string.Concat(
                            conteudo,
                            Environment.NewLine);

                    var lines = string.Join(
                        Environment.NewLine, 
                        File.ReadAllLines(
                            fileEmpregador));

                    conteudo = string.Concat(
                        conteudo,
                        lines);
                }

                return conteudo;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Collective or Individual.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<MatriculaResult> GetMatriculas()
        {
            try
            {
                if (this._filesMatricula is null ||
                    this._filesMatricula.Count == 0)
                    return null;

                var matriculasResult = new List<MatriculaResult>();

                foreach (var fileMatricula in this._filesMatricula)
                {
                    var lines = File.ReadAllLines(
                        fileMatricula);

                    foreach (var line in lines)
                    {
                        matriculasResult.Add(
                            new MatriculaResult()
                            {
                                Matricula = line.Substring(0, 8).Trim(),
                                Nome = line.Substring(9, 35).Trim(),
                                DataNascimento = line.Substring(45, 10).Trim(),
                                Cep = line.Substring(58, 10).Trim(),
                                Email = line.Substring(69, 28).Trim(),
                                Telefone = line.Substring(98, 27).Trim(),
                                Cpf = line.Substring(126, 14).Trim(),
                                DataDemissao = line.Substring(141, 10).Trim(),
                                DataAdmissao = line.Substring(152, 10).Trim(),
                                Complemento = line.Substring(163, 23).Trim(),
                                NumeroLogradouro = line.Substring(185, 6).Trim(),
                                Bairro = line.Substring(192, 20).Trim(),
                                Logradouro = line.Substring(213, 30).Trim(),
                                Cidade = line.Substring(244, 18).Trim(),
                                Uf = line.Substring(263, 2).Trim(),
                                NumeroCtps = line.Substring(266, 9).Trim(),
                                SerieCtps = line.Substring(276, 6).Trim(),
                                UfCtps = line.Substring(283, 2).Trim(),
                                Rg = line.Substring(286, 15).Trim(),
                                Cnpj = line.Substring(304, 18).Trim(),
                                DescricaoCargo = line.Substring(323, 30).Trim(),
                                DescricaoSetor = line.Substring(353, 25).Trim(),
                                FormaPagamento = line.Substring(393, 1).Trim(),
                                Banco = line.Substring(395, 4).Trim(),
                                Agencia = line.Substring(400, 5).Trim(),
                                Conta = line.Substring(406, 12).Trim(),
                                DvConta = line.Substring(419, 1).Trim(),
                                SalarioNominal = line.Substring(421, 13).Trim(),
                            });
                    }
                }

                return matriculasResult;
            }
            catch
            {
                throw;
            }
        }

        public string GetConteudoMatriculas()
        {
            try
            {
                if (this._filesMatricula is null ||
                    this._filesMatricula.Count == 0)
                    return null;

                var conteudo = string.Empty;

                foreach (var fileMatricula in this._filesMatricula)
                {
                    if (!string.IsNullOrEmpty(conteudo))
                        conteudo = string.Concat(
                            conteudo,
                            Environment.NewLine);

                    var lines = string.Join(
                        Environment.NewLine,
                        File.ReadAllLines(
                            fileMatricula));

                    conteudo = string.Concat(
                        conteudo,
                        lines);
                }

                return conteudo;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Collective or Individual.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<DemonstrativoPagamentoResult> GetDemonstrativosPagamento()
        {
            try
            {
                if (this._filesDemonstrativoPagamento is null ||
                    this._filesDemonstrativoPagamento.Count == 0)
                    return null;

                var demonstrativosPagamentoResult = new List<DemonstrativoPagamentoResult>();

                var demonstrativoPagamento = new DemonstrativoPagamentoResult();

                foreach (var fileDemonstrativo in this._filesDemonstrativoPagamento)
                {
                    var lines = File.ReadAllLines(fileDemonstrativo);

                    foreach (var line in lines)
                    {
                        if (line.Substring(0, 1) == "1")
                        {
                            demonstrativoPagamento = new DemonstrativoPagamentoResult
                            {
                                Competencia = line.Substring(1, 7).Trim(),
                                RazaoSocial = line.Substring(31, 40).Trim(),
                            };
                        }
                        else if (line.Substring(0, 1) == "2")
                        {
                            demonstrativoPagamento.Matricula = line.Substring(1, 10).Trim();
                            demonstrativoPagamento.Nome = line.Substring(11, 40).Trim();
                            demonstrativoPagamento.DescricaoCargo = line.Substring(51, 30).Trim();
                            demonstrativoPagamento.DescricaoSetor = line.Substring(81, 30).Trim();
                            demonstrativoPagamento.NumeroCtps = line.Substring(113, 7).Trim();
                            demonstrativoPagamento.SerieCtps = line.Substring(121, 4).Trim();
                            demonstrativoPagamento.UfCtps = line.Substring(126, 2).Trim();
                            demonstrativoPagamento.DataAdmissao = line.Substring(131, 10).Trim();
                            demonstrativoPagamento.Ir = line.Substring(141, 2).Trim();
                            demonstrativoPagamento.Sf = line.Substring(143, 2).Trim();
                            demonstrativoPagamento.SalarioNominal = line.Substring(145, 10).Trim();
                            demonstrativoPagamento.Banco = line.Substring(155, 3).Trim();
                            demonstrativoPagamento.Agencia = line.Substring(158, 5).Trim();
                            demonstrativoPagamento.Conta = line.Substring(163, 13).Trim();
                        }
                        else if (line.Substring(0, 1) == "3")
                        {
                            if (demonstrativoPagamento.Eventos is null)
                            {
                                demonstrativoPagamento.Eventos = new List<DemonstrativoPagamentoEventoResult>();
                            }

                            demonstrativoPagamento.Eventos.Add(new DemonstrativoPagamentoEventoResult
                            {
                                Codigo = line.Substring(1, 5).Trim(),
                                Descricao = line.Substring(6, 30).Trim(),
                                Referencia = line.Substring(36, 6).Trim(),
                                Valor = line.Substring(42, 10).Trim(),
                                Tipo = line.Substring(52, 1).Trim(),
                            });
                        }
                        else if (line.Substring(0, 1) == "4")
                        {
                            demonstrativoPagamento.BaseInss = line.Substring(1, 10).Trim();
                            demonstrativoPagamento.BaseIrrf = line.Substring(11, 10).Trim();
                            demonstrativoPagamento.BaseFgts = line.Substring(21, 10).Trim();
                            demonstrativoPagamento.ValorFgts = line.Substring(31, 10).Trim();
                            demonstrativoPagamento.TotalVencimentos = line.Substring(41, 10).Trim();
                            demonstrativoPagamento.TotalDescontos = line.Substring(51, 10).Trim();
                            demonstrativoPagamento.TotalLiquido = line.Substring(61, 10).Trim();
                        }
                        else if (line.Substring(0, 1) == "5")
                        {
                            demonstrativosPagamentoResult.Add(
                                demonstrativoPagamento);
                        }
                    }
                }

                return demonstrativosPagamentoResult;
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Collective or Individual.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<EspelhoPontoResult> GetEspelhosPonto()
        {
            try
            {
                if (this._filesEspelhoPonto is null ||
                    this._filesEspelhoPonto.Count == 0)
                    return null;

                var espelhosPontoResult = new List<EspelhoPontoResult>();

                var espelhoPonto = default(EspelhoPontoResult);

                foreach (var fileEspelhoPonto in this._filesEspelhoPonto)
                {
                    //var lines = File.ReadAllLines(
                    //    fileEspelhoPonto,
                    //    Encoding.GetEncoding(
                    //        "ISO-8859-1"));

                    var lines = File.ReadAllLines(
                        fileEspelhoPonto);

                    //using (var streamReader = new StreamReader(
                    //    fileEspelho,
                    //    Encoding.GetEncoding(
                    //        "iso-8859-1")))
                    //{
                    //    lines = streamReader.ReadToEnd();
                    //}

                    string competencia = string.Empty;

                    foreach (var line in lines)
                    {
                        if (line.Substring(0, 1) == "1")
                        {
                            espelhoPonto = new EspelhoPontoResult
                            {
                                Matricula = line.Substring(1, 10).Trim(),
                                Nome = line.Substring(11, 40).Trim(),
                                Competencia = line.Substring(134, 7).Trim(),
                            };

                            competencia = espelhoPonto.Competencia;
                        }
                        else if (line.Substring(0, 1) == "2")
                        {
                            var espelhoPontoMarcacaoResult = new EspelhoPontoMarcacaoResult
                            {
                                Data = Convert.ToDateTime(
                                    string.Concat(
                                        Convert.ToInt32(
                                            line.Substring(1, 2).Trim()).ToString("00"),
                                        "/",
                                        competencia)),
                                Marcacao = line.Substring(8, 82).Trim(),
                                HorasTrabalhadas = line.Substring(90, 5).Trim(),
                                HorasFaltas = line.Substring(96, 5).Trim(),
                                HE050 = line.Substring(102, 5).Trim(),
                                HE070 = line.Substring(108, 5).Trim(),
                                HE100 = line.Substring(114, 5).Trim(),
                                CreditoBH = line.Substring(120, 5).Trim(),
                                DebitoBH = line.Substring(126, 5).Trim(),
                            };

                            espelhoPonto.Marcacoes ??= new List<EspelhoPontoMarcacaoResult>();

                            espelhoPonto.Marcacoes.Add(
                                espelhoPontoMarcacaoResult);
                        }
                        else if (line.Substring(0, 1) == "3")
                        {
                            espelhoPonto.TotalHE050 = line.Substring(1, 10).Trim();
                            espelhoPonto.TotalHE070 = line.Substring(11, 10).Trim();
                            espelhoPonto.TotalHE100 = line.Substring(21, 10).Trim();
                            espelhoPonto.TotalAdicionalNoturno = line.Substring(31, 10).Trim();
                            espelhoPonto.TotalAtestado = line.Substring(41, 10).Trim();
                            espelhoPonto.TotalPaternidade = line.Substring(51, 10).Trim();
                            espelhoPonto.TotalSeguro = line.Substring(61, 10).Trim();
                            espelhoPonto.TotalFaltas = line.Substring(71, 10).Trim();
                            espelhoPonto.TotalFaltasJustificadas = line.Substring(81, 10).Trim();
                            espelhoPonto.TotalAtrasos = line.Substring(91, 10).Trim();
                            espelhoPonto.TotalCreditoBH = line.Substring(101, 10).Trim();
                            espelhoPonto.TotalDebitoBH = line.Substring(111, 10).Trim();

                            espelhoPonto.TotalSaldoBH = line.Substring(121, 10).Replace(
                                "+",
                                string.Empty).Trim();

                            if (espelhoPonto.TotalSaldoBH.IndexOf("-") > -1)
                            {
                                espelhoPonto.TotalSaldoBH = line.Substring(121, 10).Replace(
                                    "-",
                                    string.Empty).Trim();

                                espelhoPonto.TotalSaldoBH = string.Concat(
                                    "-",
                                    espelhoPonto.TotalSaldoBH);
                            }

                            espelhoPonto.TotalDispensaNaoRemunerada = line.Substring(131, 10).Trim();
                            espelhoPonto.TotalGratAdFech = line.Substring(141).Trim();

                            espelhosPontoResult.Add(
                                espelhoPonto);
                        }
                    }
                }

                return espelhosPontoResult;
            }
            catch
            {
                throw;
            }
        }
    }
}