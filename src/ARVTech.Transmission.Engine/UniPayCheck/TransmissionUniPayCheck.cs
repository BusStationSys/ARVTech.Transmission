namespace ARVTech.Transmission.Engine.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using ARVTech.Transmission.Engine.UniPayCheck.Results;

    public class TransmissionUniPayCheck
    {
        private readonly bool _isDirectory = false;
        private readonly bool _isFile = false;

        private readonly string _pathDirectoryOrFileName;

        private readonly List<string> _filesDemonstrativoPagamento = null;
        private readonly List<string> _filesEspelhoPonto = null;

        private readonly string _searchPatternDemonstrativoPagamento = "Contracheque*.txt";
        private readonly string _searchPatternEspelhoPonto = "EspelhoPonto*.txt";

        public bool IsDirectory
        {
            get
            {
                return this._isDirectory;
            }
        }

        public bool IsFile
        {
            get
            {
                return this._isFile;
            }
        }

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
                else if (Directory.Exists(pathDirectoryOrFileName)) // Is Folder (Coletive).
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
                    throw new FileNotFoundException($@"Arquivo {pathDirectoryOrFileName} não encontrado.");
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
                    throw new NullReferenceException(
                        nameof(
                            this._filesDemonstrativoPagamento));
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
                            demonstrativoPagamento.Cargo = line.Substring(51, 30).Trim();
                            demonstrativoPagamento.Setor = line.Substring(81, 30).Trim();
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
                                demonstrativoPagamento.Eventos = new List<Evento>();
                            }

                            demonstrativoPagamento.Eventos.Add(new Evento
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
        public IEnumerable<DemonstrativoPagamentoResult> GetEspelhosPonto()
        {
            try
            {
                if (this._filesEspelhoPonto is null ||
                    this._filesEspelhoPonto.Count == 0)
                {
                    throw new NullReferenceException(
                        nameof(
                            this._filesEspelhoPonto));
                }

                return null;

                //var espelhosPontoResult = new List<espelhopontoResult>();

                //var demonstrativoPagamento = new DemonstrativoPagamentoResult();

                //foreach (var fileDemonstrativo in this._filesDemonstrativoPagamento)
                //{
                //    var lines = File.ReadAllLines(fileDemonstrativo);

                //    foreach (var line in lines)
                //    {
                //        if (line.Substring(0, 1) == "1")
                //        {
                //            demonstrativoPagamento = new DemonstrativoPagamentoResult
                //            {
                //                Competencia = line.Substring(1, 7).Trim(),
                //                RazaoSocial = line.Substring(31, 40).Trim(),
                //            };
                //        }
                //        else if (line.Substring(0, 1) == "2")
                //        {
                //            demonstrativoPagamento.Matricula = line.Substring(1, 10).Trim();
                //            demonstrativoPagamento.Nome = line.Substring(11, 40).Trim();
                //            demonstrativoPagamento.Cargo = line.Substring(51, 30).Trim();
                //            demonstrativoPagamento.Setor = line.Substring(81, 30).Trim();
                //            demonstrativoPagamento.NumeroCtps = line.Substring(111, 9).Trim();
                //            demonstrativoPagamento.SerieCtps = line.Substring(121, 4).Trim();
                //            demonstrativoPagamento.UfCtps = line.Substring(126, 2).Trim();
                //            demonstrativoPagamento.DataAdmissao = line.Substring(131, 10).Trim();
                //            demonstrativoPagamento.Ir = line.Substring(141, 2).Trim();
                //            demonstrativoPagamento.Sf = line.Substring(143, 2).Trim();
                //            demonstrativoPagamento.SalarioNominal = line.Substring(145, 10).Trim();
                //            demonstrativoPagamento.Banco = line.Substring(155, 3).Trim();
                //            demonstrativoPagamento.Agencia = line.Substring(158, 5).Trim();
                //            demonstrativoPagamento.Conta = line.Substring(163, 13).Trim();
                //        }
                //        else if (line.Substring(0, 1) == "3")
                //        {
                //            if (demonstrativoPagamento.Eventos is null)
                //            {
                //                demonstrativoPagamento.Eventos = new List<Evento>();
                //            }

                //            demonstrativoPagamento.Eventos.Add(new Evento
                //            {
                //                Codigo = line.Substring(1, 5).Trim(),
                //                Descricao = line.Substring(6, 30).Trim(),
                //                Referencia = line.Substring(36, 6).Trim(),
                //                Valor = line.Substring(42, 10).Trim(),
                //                Tipo = line.Substring(52, 1).Trim(),
                //            });
                //        }
                //        else if (line.Substring(0, 1) == "4")
                //        {

                //        }
                //        else if (line.Substring(0, 1) == "5")
                //        {
                //            demonstrativosPagamentoResult.Add(
                //                demonstrativoPagamento);
                //        }
                //    }
                //}

                //return demonstrativosPagamentoResult;
            }
            catch
            {
                throw;
            }
        }
    }
}