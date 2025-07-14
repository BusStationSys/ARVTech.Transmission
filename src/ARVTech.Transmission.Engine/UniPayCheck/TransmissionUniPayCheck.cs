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
        public string GetConteudoDemonstrativosPagamento()
        {
            try
            {
                if (this._filesDemonstrativoPagamento is null ||
                    this._filesDemonstrativoPagamento.Count == 0)
                    return null;

                var conteudo = string.Empty;

                foreach (var fileDemonstrativoPagamento in this._filesDemonstrativoPagamento)
                {
                    if (!string.IsNullOrEmpty(conteudo))
                        conteudo = string.Concat(
                            conteudo,
                            Environment.NewLine);

                    var lines = string.Join(
                        Environment.NewLine,
                        File.ReadAllLines(
                            fileDemonstrativoPagamento));

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
        public string GetConteudoEspelhosPonto()
        {
            try
            {
                if (this._filesEspelhoPonto is null ||
                    this._filesEspelhoPonto.Count == 0)
                    return null;

                var conteudo = string.Empty;

                foreach (var fileEspelhoPonto in this._filesEspelhoPonto)
                {
                    if (!string.IsNullOrEmpty(conteudo))
                        conteudo = string.Concat(
                            conteudo,
                            Environment.NewLine);

                    var lines = string.Join(
                        Environment.NewLine,
                        File.ReadAllLines(
                            fileEspelhoPonto));

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
    }
}