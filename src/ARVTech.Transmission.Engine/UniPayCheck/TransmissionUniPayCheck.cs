namespace ARVTech.Transmission.Engine.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using ARVTech.Transmission.Engine.UniPayCheck.Results;

    public class TransmissionUniPayCheck
    {
        private readonly bool _isDirectory = false;
        private readonly bool _isFile = false;

        private readonly string _pathDirectoryOrFileName;

        private readonly List<string> _filesDemonstrativoPagamento;
        private readonly List<string> _filesEspelhoPonto;

        private readonly string _searchPatternDemonstrativoPagamento = "DemonstrativoPagamento*.txt";
        private readonly string _searchPatternEspelhoPonto = "EspelhoPonto*.txt";

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
            try
            {
                if (string.IsNullOrEmpty(pathDirectoryOrFileName))
                {
                    throw new ArgumentNullException(
                        nameof(
                            pathDirectoryOrFileName));
                }

                this._filesDemonstrativoPagamento = new List<string>();
                this._filesEspelhoPonto = new List<string>();

                if (File.Exists(pathDirectoryOrFileName))   // Is File (Individual).
                {
                    this._filesDemonstrativoPagamento.Add(
                        pathDirectoryOrFileName);

                    this._filesEspelhoPonto.Add(
                        pathDirectoryOrFileName);

                    this._isFile = true;
                }
                else if (Directory.Exists(pathDirectoryOrFileName)) // Is Folder (Collective).
                {
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
                {
                    throw new FileNotFoundException($@"Diretório ou Arquivo {pathDirectoryOrFileName} não encontrado.");
                }

                this._pathDirectoryOrFileName = pathDirectoryOrFileName;
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
                {
                    return null;
                    //throw new NullReferenceException(
                    //    nameof(
                    //        this._filesDemonstrativoPagamento));
                }

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
                {
                    return null;
                    //throw new NullReferenceException(
                    //    nameof(
                    //        this._filesEspelhoPonto));
                }

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
                                DescricaoSetor = line.Substring(57, 34).Trim(),
                                CargaHoraria = line.Substring(113, 6).Trim().Replace(":", "."),
                                Cnpj = this.replaceSetorByCnpj(
                                    line.Substring(121, 13).Trim()),
                                RazaoSocial = this.replaceSetorByRazaoSocial(
                                    line.Substring(121, 13).Trim()),
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
                                HorasNormais = line.Substring(90, 5).Trim(),
                                HorasFaltas = line.Substring(96, 5).Trim(),
                                HE050 = line.Substring(102, 5).Trim(),
                                HE070 = line.Substring(108, 5).Trim(),
                                HE100 = line.Substring(114, 5).Trim(),
                                CreditoBH = line.Substring(120, 5).Trim(),
                                DebitoBH = line.Substring(126, 5).Trim(),
                            };

                            if (espelhoPonto.Marcacoes is null)
                                espelhoPonto.Marcacoes = new List<EspelhoPontoMarcacaoResult>();

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
                            espelhoPonto.TotalSaldoBH = line.Substring(121, 10).Trim();
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string replaceSetorByCnpj(string content)
        {
            if (content.Trim().ToUpper() == "MATRIZ")
                return "07718633000189";

            return string.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        private string replaceSetorByRazaoSocial(string content)
        {
            if (content.Trim().ToUpper() == "MATRIZ")
                return "UNIDASUL DIST ALIMENTICIA S.A. MTZ";

            return string.Empty;
        }
    }
}