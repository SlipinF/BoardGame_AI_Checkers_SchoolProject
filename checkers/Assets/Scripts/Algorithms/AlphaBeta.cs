using System;
using System.Collections.Generic;

// Minimalt AlphaBeta-exempel
namespace AlphaBeta
{
  class Tree
  {
    public string name;
    public int value;
    public List<Tree> children;

    public Tree(string name) {
      this.name = name;
    }

    public void Add(Tree child) {
      if (children == null)
        children = new List<Tree>();
      children.Add(child);
    }
  }

  class MainClass
  {

    public static int alphabeta(Tree node, int depth, int α, int β, bool maximizingPlayer)
    {
      int value;
      if (depth == 0 || node.value == Int32.MaxValue || node.value == Int32.MinValue)
        return node.value;

      if (maximizingPlayer)
      {
        value = Int32.MinValue;
        foreach (Tree child in node.children)
        {
          value = Math.Max(value, alphabeta(child, depth - 1, α, β, false));
          α = Math.Max(α, value);
          if (α >= β)
            break;  
        }
        return value;
      }
      else
      {
        value = Int32.MaxValue;
        foreach (Tree child in node.children)
        {
          value = Math.Min(value, alphabeta(child, depth - 1, α, β, true));
          β = Math.Min(β, value);
          if (α >= β)
            break;  
        }
        return value;
      }
    }

    public static void Main(string[] args)
    {
      Tree A = new Tree("A");
      Tree B = new Tree("B");
      Tree C = new Tree("C");
      Tree D = new Tree("D");
      Tree E = new Tree("E");
      Tree F = new Tree("F");
      Tree G = new Tree("G");
      Tree H = new Tree("H");
      Tree I = new Tree("I");
      Tree J = new Tree("J");
      Tree K = new Tree("K");
      Tree L = new Tree("L");
      Tree M = new Tree("M");
      Tree N = new Tree("N");
      Tree O = new Tree("O");
      Tree P = new Tree("P");
      Tree Q = new Tree("Q");
      Tree R = new Tree("R");
      Tree S = new Tree("S");
      Tree T = new Tree("T");
      Tree U = new Tree("U");
      Tree V = new Tree("V");
      Tree W = new Tree("W");
      Tree X = new Tree("X");
      Tree Y = new Tree("Y");
      Tree Z = new Tree("Z");
      Tree AA = new Tree("AA");
      Tree AB = new Tree("AB");
      Tree AC = new Tree("AC");
      Tree AD = new Tree("AD");
      Tree AE = new Tree("AE");
      Tree AF = new Tree("AF");
      Tree AG = new Tree("AG");

      A.Add(B);
      A.Add(C);
      A.Add(D);
      B.Add(E);
      B.Add(F);
      E.Add(K);
      E.Add(L);
      K.Add(T);
      K.Add(U);
      L.Add(V);
      L.Add(W);
      L.Add(X);
      F.Add(M);
      M.Add(Y);
      C.Add(G);
      C.Add(H);
      G.Add(N);
      G.Add(O);
      N.Add(Z);
      O.Add(AA);
      O.Add(AB);
      H.Add(P);
      P.Add(AC);
      D.Add(I);
      D.Add(J);
      I.Add(Q);
      Q.Add(AD);
      J.Add(R);
      J.Add(S);
      R.Add(AE);
      R.Add(AF);
      S.Add(AG);

      T.value = 5;
      U.value = 6;
      V.value = 7;
      W.value = 4;
      X.value = 5;
      Y.value = 3;
      Z.value = 6;
      AA.value = 6;
      AB.value = 9;
      AC.value = 7;
      AD.value = 5;
      AE.value = 9;
      AF.value = 8;
      AG.value = 6;

      alphabeta(A, 4, Int32.MinValue, Int32.MaxValue, true);
    }
  }
}
