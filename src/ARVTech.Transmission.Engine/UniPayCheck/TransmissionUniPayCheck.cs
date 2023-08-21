namespace ARVTech.Transmission.Engine.UniPayCheck
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using ARVTech.Shared.Extensions;
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

        private readonly List<string> _ocorrenciasEspelhoPonto = new()
        {
            "ATESTADO",
            "COMPENSADO",
            "DEBITO BH",
            "DESCANSO",
            "NORMAL",
        };

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
                    throw new NullReferenceException(
                        nameof(
                            this._filesEspelhoPonto));
                }

                var espelhosPontoResult = new List<EspelhoPontoResult>();

                var espelhoPonto = default(EspelhoPontoResult);

                foreach (var fileEspelhoPonto in this._filesEspelhoPonto)
                {
                    var lines = File.ReadAllLines(
                        fileEspelhoPonto,
                        Encoding.GetEncoding(
                            "ISO-8859-1"));

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
                                Matricula = line.Substring(1, 10).Trim().TreatStringWithAccent(),
                                Nome = line.Substring(11, 40).Trim().TreatStringWithAccent(),
                                DescricaoSetor = line.Substring(57, 34).Trim().TreatStringWithAccent(),
                                CargaHoraria = line.Substring(113, 6).Trim().Replace(":", "."),
                                Cnpj = this.replaceSetorByCnpj(
                                    line.Substring(121, 13).Trim()),
                                Competencia = line.Substring(134, 8).Trim(),
                            };

                            competencia = espelhoPonto.Competencia;
                        }
                        else if (line.Substring(0, 1) == "2")
                        {
                            var espelhoPontoMarcacaoResult = new EspelhoPontoMarcacaoResult
                            {
                                DataMarcacao = Convert.ToDateTime(
                                    string.Concat(
                                        Convert.ToInt32(
                                            line.Substring(1, 2).Trim()).ToString("00"),
                                        "/",
                                        competencia)),
                            };

                            string marcacao = line.Substring(8, 82).Trim();

                            int positionOcorrenciaMarcacao = getPositionOcorrenciaMarcacao(marcacao);

                            if (positionOcorrenciaMarcacao >= 0)
                            {
                                marcacao = marcacao.Substring(0, positionOcorrenciaMarcacao).Trim();
                            }

                            if (!string.IsNullOrEmpty(marcacao))
                            {
                                espelhoPontoMarcacaoResult.HorarioMarcacao1 = marcacao.Substring(0, 5).Trim();
                                espelhoPontoMarcacaoResult.HorarioMarcacao2 = marcacao.Substring(6, 5).Trim();
                                espelhoPontoMarcacaoResult.HorarioMarcacao3 = marcacao.Substring(12, 5).Trim();
                                espelhoPontoMarcacaoResult.HorarioMarcacao4 = marcacao.Substring(18, 5).Trim();
                            }

                            if (espelhoPonto.Marcacoes is null)
                                espelhoPonto.Marcacoes = new List<EspelhoPontoMarcacaoResult>();

                            espelhoPonto.Marcacoes.Add(
                                espelhoPontoMarcacaoResult);
                        }
                        else if (line.Substring(0, 1) == "3")
                        {
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
        /// <param name="marcacao"></param>
        /// <returns></returns>
        private int getPositionOcorrenciaMarcacao(string marcacao)
        {
            foreach (var ocorrenciaEspelhoPonto in this._ocorrenciasEspelhoPonto)
            {
                if (marcacao.ToUpper().IndexOf(ocorrenciaEspelhoPonto.ToUpper()) >= 0)
                {
                    return marcacao.ToUpper().IndexOf(ocorrenciaEspelhoPonto.ToUpper());
                }
            }

            return -1;
        }
    }
}