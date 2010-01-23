using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Games.mx
{
	class Ship
	{
		int i_Cargo; //Cargo 0 - 100 %
		UInt64 i_Money; //Geld in Credits
		int i_Damage; //beschädigung
		int i_Missiles; //Anzahl Missiliesc
		int i_Mines; //Anzahl Minen
		int i_Shields; //Anzahl Schilder
		int i_Torpedos; //Anzahl Topedos

		bool i_MatterCannon;
		bool i_ProtonGun;
		bool i_AfterBurner;
	}
}
