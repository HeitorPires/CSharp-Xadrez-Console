using tabuleiro;

namespace xadrez
{
    internal class Rei : Peca
    {
        public Rei(Tabuleiro tab, Cor cor) : base(cor, tab)
        {
        }

        public override string ToString()
        {
            return "R";
        }

        private bool PodeMover(Posicao pos)
        {
            Peca p = Tab.GetPeca(pos);
            return p == null || p.Cor != Cor;
        }

        public override bool[,] MovimentosPossiveis()
        {
            bool[,] mat = new bool[Tab.Linhas, Tab.Colunas];
            Posicao pos = new(0, 0);

            //Norte
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna);
            if(Tab.PosicalValida(pos) && PodeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            //Nordeste
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna + 1);
            if (Tab.PosicalValida(pos) && PodeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            //Leste
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna + 1);
            if (Tab.PosicalValida(pos) && PodeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            //Sudeste
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna + 1);
            if (Tab.PosicalValida(pos) && PodeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            //Sul
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna);
            if (Tab.PosicalValida(pos) && PodeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            //Sudoeste
            pos.DefinirValores(Posicao.Linha + 1, Posicao.Coluna - 1);
            if (Tab.PosicalValida(pos) && PodeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            //Oeste
            pos.DefinirValores(Posicao.Linha, Posicao.Coluna - 1);
            if (Tab.PosicalValida(pos) && PodeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            //Noroeste
            pos.DefinirValores(Posicao.Linha - 1, Posicao.Coluna - 1);
            if (Tab.PosicalValida(pos) && PodeMover(pos))
                mat[pos.Linha, pos.Coluna] = true;

            return mat;
        }
    }
}
