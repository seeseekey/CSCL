//
//  ToAscii.cs
//
//  Copyright (c) 2011, 2012 by seeseekey <seeseekey@googlemail.com>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Text;

namespace CSCL.Graphic
{
	public partial class gtImage
	{
		/// <summary>
		/// Computes the avarange grayscale value of a certain bitmap region.
		/// </summary>
		/// <param name="grayscaleBitmap">A grayscale bitmap.</param>
		/// <param name="rectangle">Defines a region.</param>
		/// <returns></returns>
		public static int GetAvarageGrayscale(gtImage grayscaleBitmap, System.Drawing.Rectangle rectangle)
		{
			int average=0;

			try
			{
				for(int y=rectangle.Y; y<rectangle.Bottom; y++)
					for(int x=rectangle.X; x<rectangle.Right; x++)
					{
						average+=grayscaleBitmap.GetPixel(x, y).R;
					}
			}
			catch(System.Exception ex)
			{
				return 128;
			}

			return average/(rectangle.Width*rectangle.Height);
		}

		/// <summary>
		/// Computes the difference between to different bitmap regions.
		/// </summary>
		/// <param name="grayscaleBitmapA">First bitmap.</param>
		/// <param name="compareRectA">First bitmap region.</param>
		/// <param name="grayscaleBitmapB">Secound bitmap</param>
		/// <param name="compareRectB">Secound bitmap region.</param>
		/// <returns></returns>
		public static int ComputeDistance(gtImage grayscaleBitmapA,
							System.Drawing.Rectangle compareRectA,
							gtImage grayscaleBitmapB,
							System.Drawing.Rectangle compareRectB)
		{
			int averageA=GetAvarageGrayscale(grayscaleBitmapA, compareRectA);
			int averageB=GetAvarageGrayscale(grayscaleBitmapB, compareRectB);

			return System.Math.Abs(averageA-averageB);
		}

		/// <summary>
		/// Applies an edge filter to the bitmap.
		/// </summary>
		/// <param name="bitmap">A bitmap.</param>
		/// <returns>Edge bitmap.</returns>
		public static gtImage DetectEdges(gtImage bitmap)
		{
			gtImage grayscaleMap=bitmap.ConvertToGray();

			gtImage edgeMap=new gtImage(grayscaleMap.Width, grayscaleMap.Height, Format.RGB);

			for(int y=1; y<edgeMap.Height-1; y++)
			{
				for(int x=1; x<edgeMap.Width-1; x++)
				{
					int sum=System.Math.Abs(grayscaleMap.GetPixel(x-1, y-1).R+grayscaleMap.GetPixel(x, y-1).R+grayscaleMap.GetPixel(x+1, y-1).R
						-grayscaleMap.GetPixel(x-1, y+1).R-grayscaleMap.GetPixel(x, y+1).R-grayscaleMap.GetPixel(x+1, y+1).R);

					System.Drawing.Color z1=grayscaleMap.GetPixel(x-1, y-1);
					System.Drawing.Color z2=grayscaleMap.GetPixel(x, y-1);
					System.Drawing.Color z3=grayscaleMap.GetPixel(x+1, y-1);
					System.Drawing.Color z4=grayscaleMap.GetPixel(x-1, y+1);
					System.Drawing.Color z5=grayscaleMap.GetPixel(x, y+1);
					System.Drawing.Color z6=grayscaleMap.GetPixel(x+1, y+1);

					int foo=z1.R+z2.R+z3.R-z4.R-z5.R-z6.R;
					if(sum>255)
						sum=255;

					sum=255-sum;

					edgeMap.SetPixel(x, y, System.Drawing.Color.FromArgb(255, sum, sum, sum));
				}
			}

			return edgeMap;
		}

		/// <summary>
		/// Converts a bitmap to ascii art.
		/// </summary>
		/// <param name="bitmap">A bitmap.</param>
		/// <returns>Ascii art.</returns>
		/// <example>
		/* 0:::::::::::::::::::::::::::::$MMMMM%:::::::::::::::::::::::::::::::::::::M
			%                            (#''` !M8                                    M
			%                    :QMMMMMMMM849640MMM@(                                M
			%                 "MMMMMMM4*' HM:  !MB#MMMMMM$                            M
			%               'MMMM3        ''$844$    `6MMMMM'                         M
			% '#MNN#H@@@@@@HMMM!          '''`           $MMM3                        M
			%1M@99889QB81!!M*M4'`                          @QM:                       M
			%1Q'"B000008MMM*`MM:`''`                       !M@3                :&$;`  M
			%0`  $`      !B  `*MM6  ''''`                 'MM(!:            $MMMMMMMMMM
			%!&  `8'     #`    ''1MM@H3'`'''`         '$MMMH   %          4M$''!@BMMM4M
			% ;0   46   (3       `''''1#@HBH#&$&$448MMMM0'     :'       (M& ':@BQ@"   M
			%  !B(16BMQ`B                       :894:          `*     'MH   B8Q0`     M
			%    &MM` *M&                      !4               ;:"Q#M#'  !@3Q`       M
			%      4MM;&`                      "                (01'`'''`"Q0"         M
			%        :MN                      `(                `'''`  '84B'          M
			%                                 &                 `` `''QB09`           M
			%          !                     :(                 '389088@;             M
			%          1:                   '9                  !"6QQQ!               M
			%          `#                  :B                  `84'                   M
			%           (#                04                   &'                     M
			%            1M6           `9B!                   $:                      M
			%             !M#006(:'"6490`                   `Q:                       M
			%               4B! :(1!                      `48`                        M
			%                 4@9'                      ;@B!                          M
			%                   '9QB0!              ;8QB3                             M
			%                       `&88909444099898%                                 M
			0:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::M 
		*/
		/// </example>
		public string ToAscii()
		{
			gtImage edgeMap=DetectEdges(this);

			string output="";

			for(int y=0; y<edgeMap.Height; y+=13)
			{
				for(int x=0; x<edgeMap.Width; x+=8)
				{
					int minDistance=999999999;
					char minChar=' ';

					for(int i=32; i<127; i++)
					{
						char c=(char)i;
						string str=""+c;

						gtImage characterMap=new gtImage(8, 13, Format.RGB);
						//System.Drawing.Graphics g=System.Drawing.Graphics.FromImage(characterMap);
						characterMap.Fill(System.Drawing.Color.White);

						//characterMap.Rendertex
						gtImage mytext=gtImage.RenderText(new System.Drawing.Font("Courier New", 10), str, System.Drawing.Color.Blue);
						characterMap.Draw(-2, -2, mytext);

						int tmp=ComputeDistance(characterMap,
							new System.Drawing.Rectangle(0, 0, (int)characterMap.Width, (int)characterMap.Height),
							edgeMap,
							new System.Drawing.Rectangle(x, y, (int)characterMap.Width, (int)characterMap.Height));

						if(tmp<minDistance)
						{
							minDistance=tmp;
							minChar=c;
						}
					}

					output+=minChar;
				}

				output+="\r\n";
			}

			return output;
		}
	}
}