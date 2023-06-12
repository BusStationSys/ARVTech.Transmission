namespace ARVTech.Transmission.Engine
{
    using System;

    /// <summary>
    /// Interface do Design Pattern Strategy com os métodos comuns a todos os Transmission Engine.
    /// </summary>
    public interface ITransmissionEngine
    {
        /// <summary>
        /// Método que será invocado por todas as classes concretas dos Transmission Engine que implementam essa interface.
        /// </summary>
        void Executar();
    }
}