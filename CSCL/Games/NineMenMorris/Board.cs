using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Games.NineMenMorris
{
	class Board
	{
		#region Enums
		/// <summary>
		/// Enum für die Positionen des Spielbrettes
		/// </summary>
		enum BoardPositions
		{
			A1, A2, A3, A4, A5, A6, A7, A8,
			B1, B2, B3, B4, B5, B6, B7, B8,
			C1, C2, C3, C4, C5, C6, C7, C8
		}

		/// <summary>
		/// Gibt an von wem der Spieler kontrolliert wird
		/// </summary>
		enum PlayerControl
		{
			Human, KI
		}

		/// <summary>
		/// Gibt an in welcher Spielphase sich der Spieler befindet
		/// Opening = Eröffnung
		/// Middlegame = Mittelspiel
		/// Endgame = Endspiel
		/// </summary>
		enum PlayerGameState { Opening, Middlegame, Endgame }

		/// <summary>
		/// Gibt den Status eines Feldes auf dem Spielbrett an.
		/// </summary>
		enum FieldState { None, Black, White };

		/// <summary>
		/// Globaler Status des Spieles
		/// </summary>
		enum GlobalGameState { OutOfGame, InGame }
		#endregion

		struct Player
		{
			PlayerControl ControlledBy;
			PlayerGameState GameState;

			byte CountStones; //Anzahl der Steine
		}

		struct BoardFieldState
		{
			BoardPositions FieldName;
			FieldState IntFieldState;
		}

		Player Player01;
		Player Player02;

		BoardFieldState[,] i_Board=new BoardFieldState[3, 3];

		BoardPositions CurrentMarkedStone;
		Player CurrentPlayer;

		bool vIsMuehle; //Wird auf true gesetzt wenn durch einen Zug eine Mühle gebildet wurde

		/// <summary>
		/// Überprüft ob ein Zug gemäß den Spielregeln erlaubt ist
		/// IsMoveValid Checktabelle
		/// A1-A2,A8      B1-B2,B8        C1-C2,C8
		/// A2-A1,A3,B2   B2-B1,B3,A2,C2  C2-C1,C3,B2
		/// A3-A2,A4      B3-B2,B4        C3-C2,C4
		/// A4-A3,A5,B4   B4-B3,B5,A4,C4  C4-C3,C5,B4
		/// A5-A4,A6      B5-B4,B6        C5-C4,C6
		/// A6-A5,A7,B6   B6-B5,B7,A6,C6  C6-C5,C7,B6
		/// A7-A6,A8      B7-B6,B8        C7-C6,C8
		/// A8-A7,A1,B8   B8-B7,B1,A8,C8  C8-C7,C1,B8
		/// </summary>
		/// <param name="Von"></param>
		/// <param name="Nach"></param>
		/// <returns></returns>
		bool IsMoveValid(BoardPositions Von, BoardPositions Nach)
		{
			switch(Von)
			{
				case BoardPositions.A1:
					{
						if(Nach==BoardPositions.A2) return true;
						if(Nach==BoardPositions.A8) return true;
						break;
					}
				case BoardPositions.A2:
					{
						if(Nach==BoardPositions.A1) return true;
						if(Nach==BoardPositions.A3) return true;
						if(Nach==BoardPositions.B2) return true;
						break;
					}
				case BoardPositions.A3:
					{
						if(Nach==BoardPositions.A2) return true;
						if(Nach==BoardPositions.A4) return true;
						break;
					}
				case BoardPositions.A4:
					{
						if(Nach==BoardPositions.A3) return true;
						if(Nach==BoardPositions.A5) return true;
						if(Nach==BoardPositions.B4) return true;
						break;
					}
			}

			return false;


		}

		/// <summary>
		/// IsMuehle gibt an ob ein bestimmter Stein Bestandteil einer Mühle ist
		/// </summary>
		/// <param name="Position"></param>
		/// <returns></returns>
		bool IsMuehle(BoardPositions Position)
		{
			return true;
		}
	}
}
