namespace tabuleiro
{
    internal class Peca
    {
        public Posicao Posicao { get; set; }
        public Cor Cor { get; protected set; }
        public int QteMovimentos { get; protected set; }
        public Tabuleiro Tab { get; protected set; }

        public Peca(Cor cor, Tabuleiro tab)
        {
            Cor = cor;
            Tab = tab;
            Posicao = null;
            QteMovimentos = 0;
        }
    }
}
