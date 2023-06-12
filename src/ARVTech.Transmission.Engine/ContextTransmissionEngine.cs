namespace ARVTech.Transmission.Engine
{
    /// <summary>
    /// Context do Design Pattern Strategy que define a interface de interesse do PaymentsTool Engine.
    /// </summary>
    public class ContextTransmissionEngine
    {
        private ITransmissionEngine _transmissionEngine;

        /// <summary>
        /// Construtor da classe Context.
        /// </summary>
        public ContextTransmissionEngine()
        { }

        /// <summary>
        /// Construtor da classe Context passando um objeto <see cref="ITransmissionEngine"/>.
        /// </summary>
        /// <param name="transmissionEngine"></param>
        public ContextTransmissionEngine(ITransmissionEngine transmissionEngine)
        {
            this._transmissionEngine = transmissionEngine;
        }

        /// <summary>
        /// Método que permite substituir um objeto <see cref="ITransmissionEngine"/> em tempo de execução.
        /// </summary>
        /// <param name="transmissionEngine"></param>
        public void SetIntegrador(ITransmissionEngine transmissionEngine)
        {
            this._transmissionEngine = transmissionEngine;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Executar()
        {
            this._transmissionEngine.Executar();
        }
    }
}