using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Cutter
{
	public static class Rendezesek
	{
		static void Rendez1D(int[] arr2, int p, int r)
		{
			int Low, High, MidValue;
			Low = p;
			High = r;
			MidValue = arr2[(p + r) / 2];
			do
			{
				while (arr2[Low] < MidValue) ++Low;
				while (arr2[High] > MidValue) --High;
				if (Low <= High)
				{
					int T = arr2[Low];
					arr2[Low] = arr2[High];
					arr2[High] = T;
					++Low;
					--High;
				}
			} while (Low <= High);
			if (p < High) Rendez1D(arr2, p, High);
			if (Low < r) Rendez1D(arr2, Low, r);
		}

		public static void SzalDarabRendezes(int[] arr, int[] hatarok, int index, bool csokkeno)
		{
			//Az index szál darabjait rendezi sorba növekvő/csökkenő érték szerint.
			int[] tempArr = new int[hatarok[1 + index] - hatarok[index]];
			Array.Copy(arr, hatarok[index], tempArr, 0, tempArr.Length);
			Array.Sort(tempArr);
			if (csokkeno) Array.Reverse(tempArr);
			Array.Copy(tempArr, 0, arr, hatarok[index], tempArr.Length);
		}

		public static void SzalDarabRendezes(int[] arr, int[] hatarok, int index)
		{
			Rendez1D(arr, hatarok[index], hatarok[1 + index] - 1);
		}

		static void Rendez2D_Cs(int[,] arr2, int row, int p, int r)
		{
			int Low, High, MidValue;
			Low = p;
			High = r;
			MidValue = arr2[row, (p + r) / 2];
			do
			{
				while (arr2[row, Low] > MidValue) ++Low;
				while (arr2[row, High] < MidValue) --High;
				if (Low <= High)
				{
					int T = arr2[row, Low];
					arr2[row, Low] = arr2[row, High];
					arr2[row, High] = T;
					++Low;
					--High;
				}
			} while (Low <= High);
			if (p < High) Rendez2D_Cs(arr2, row, p, High);
			if (Low < r) Rendez2D_Cs(arr2, row, Low, r);
		}

		public static void SzalRendez2D(int[,] tomb2d, int sordarab)
		{
			for (int i = 0; i < sordarab; ++i)
			{
				Rendez2D_Cs(tomb2d, i, 2, tomb2d[i, 0] + 1);
			}
		}


		public static void RendezHulladekCsokkeno2D(int[,] tomb2D, int sordarab)
		{
			int mintempindex;
			for (int i = 0; i < sordarab - 1; ++i)
			{
				mintempindex = i;
				for (int j = 1 + i; j < sordarab; ++j)
				{
					if (tomb2D[j, 1] > tomb2D[mintempindex, 1])
					{
						mintempindex = j;
					}
				}
				if (mintempindex > i)
				{
					SzalCsere2D(tomb2D, i, mintempindex);
				}
			}
		}

		static void SzalCsere2D(int[,] Tomb2D, int szal1, int szal2)
		{
			int mashossz = Tomb2D[szal1, 0];
			if (Tomb2D[szal2, 0] > Tomb2D[szal1, 0]) mashossz = Tomb2D[szal2, 0];
			int tmp;
			for (int i = 0; i < mashossz; ++i)
			{
				tmp = Tomb2D[szal1, 2 + i];
				Tomb2D[szal1, 2 + i] = Tomb2D[szal2, 2 + i];
				Tomb2D[szal2, 2 + i] = tmp;
			}
			tmp = Tomb2D[szal1, 0];
			Tomb2D[szal1, 0] = Tomb2D[szal2, 0];
			Tomb2D[szal2, 0] = tmp;
			tmp = Tomb2D[szal1, 1];
			Tomb2D[szal1, 1] = Tomb2D[szal2, 1];
			Tomb2D[szal2, 1] = tmp;
		}
		public static void QuickSort(int[] arr2, int p, int r)
		{//quicksort
			int Low, High;
			int MidValue;
			Low = p;
			High = r;
			MidValue = arr2[(p + r) / 2];
			do
			{
				while (arr2[Low] < MidValue) ++Low;
				while (arr2[High] > MidValue) --High;
				if (Low <= High)
				{
					int T = arr2[Low];
					arr2[Low] = arr2[High];
					arr2[High] = T;
					++Low;
					--High;
				}
			} while (Low <= High);
			if (p < High) QuickSort(arr2, p, High);
			if (Low < r) QuickSort(arr2, Low, r);
		}
	}

	public class CuttingPlan
	{
		public int Szalhossz;
		public int Szaldarab, NegyzetOsszeg, Osszeg;
		public int[] Eredmeny;
		public int[] Hatarok, Hulladekok, Darabszamok;
		public int[] KombArr;
		public int KombArrIndex = 0;
		public int[,] KettodArr;

		public CuttingPlan()
		{

		}
	}

	public class Cutter_1
	{
		CuttingPlan cuttingPlan, cuttingPlanTemp;

		public bool ende = false;
		Random rnd;

		public Cutter_1(string text)
		{
			cuttingPlan = new CuttingPlan();
			cuttingPlanTemp = new CuttingPlan();
			cuttingPlanTemp.KombArr = new int[200000];
			cuttingPlanTemp.Eredmeny = ReadFromString(text, ref cuttingPlanTemp.Szalhossz);
			rnd = new Random(30000);
		}

		static int[] ReadFromString(string text, ref int szalhossz)
		{
			string[] strt2 = text.Split(Environment.NewLine.ToCharArray());
			string[] strt3 = strt2[0].Split(',');
			if (strt3.Length < 2) throw new ApplicationException("Szálhosszt és a ráhagyást is meg kell adni \",\"(vessző)-vel elválasztva!!!");
			if (strt3.Length > 2) throw new ApplicationException("Csak egy szálhosszt és a ráhagyást lehet egy sorban megadni!!!");
			if (!int.TryParse(strt3[0], out szalhossz)) throw new ApplicationException("A szál hossza nem szám!!!");
			int rahagyas = 0;
			if (!int.TryParse(strt3[1], out rahagyas)) throw new ApplicationException("Az ráhagyás nem szám!!!");
			int[] tomb = new int[50];
			int aktindex = 0;
			for (int i = 1; i < strt2.Length; ++i)
			{
				if (strt2[i] != "")
				{
					strt3 = strt2[i].Split(',');
					if (strt3.Length < 2) throw new ApplicationException("Méretet és darabszámot is meg kell adni \",\"(vessző)-vel elválasztva!!!");
					if (strt3.Length > 2) throw new ApplicationException("Csak egy méretet és darabszámot lehet egy sorban megadni!!!");
					int meret, darab;
					if (!int.TryParse(strt3[0], out meret)) throw new ApplicationException("Az egyik méret nem szám!!!");
					meret += rahagyas;
					if (!int.TryParse(strt3[1], out darab)) throw new ApplicationException("Az egyik darabszám nem szám!!!");
					if (aktindex + darab > tomb.Length) Array.Resize(ref tomb, aktindex + darab + 50);
					for (int j = 0; j < darab; ++j) tomb[aktindex++] = meret;
				}
			}
			Array.Resize(ref tomb, aktindex);
			return tomb;
		}

		void TempToReal()
		{
			cuttingPlan.Eredmeny = (int[])cuttingPlanTemp.Eredmeny.Clone();
			cuttingPlan.Szalhossz = cuttingPlanTemp.Szalhossz;
			cuttingPlan.Szaldarab = cuttingPlanTemp.Szaldarab;
			cuttingPlan.Osszeg = cuttingPlanTemp.Osszeg;
			cuttingPlan.NegyzetOsszeg = cuttingPlanTemp.NegyzetOsszeg;
		}

		public void Manipulal(long probakszama)
		{
			int darab = cuttingPlanTemp.Eredmeny.Length;
			cuttingPlanTemp.Osszeg = 0;
			for (int i = 0; i < darab; ++i) cuttingPlanTemp.Osszeg += cuttingPlanTemp.Eredmeny[i];
			cuttingPlan.Osszeg=cuttingPlanTemp.Osszeg;
			cuttingPlanTemp.Darabszamok = new int[1 + cuttingPlanTemp.Eredmeny.Length];
			cuttingPlanTemp.Hatarok = new int[1 + cuttingPlanTemp.Eredmeny.Length];
			cuttingPlanTemp.Hulladekok = new int[1 + cuttingPlanTemp.Eredmeny.Length];
			cuttingPlanTemp.Szaldarab = 0;
			Array.Sort(cuttingPlanTemp.Eredmeny); Array.Reverse(cuttingPlanTemp.Eredmeny);
			JellemzokHosszra(cuttingPlanTemp.Eredmeny, darab, ref cuttingPlanTemp.Szalhossz, ref cuttingPlanTemp.Szaldarab, cuttingPlanTemp.Hatarok, cuttingPlanTemp.Hulladekok, cuttingPlanTemp.Darabszamok, ref cuttingPlanTemp.NegyzetOsszeg);

			cuttingPlanTemp.Eredmeny = Optimalizalo();

			for (int i = 0; i <= cuttingPlanTemp.Szaldarab; ++i)
			{
				Rendezesek.SzalDarabRendezes(cuttingPlanTemp.Eredmeny, cuttingPlanTemp.Hatarok, i, false);
			}

			cuttingPlanTemp.Eredmeny = Optimalizalo2(probakszama);
			SzaldarabEsMaxnegyzetFeltoltese(cuttingPlanTemp.Eredmeny, cuttingPlanTemp.Eredmeny.Length, cuttingPlanTemp.Szalhossz, out cuttingPlanTemp.Szaldarab, out cuttingPlanTemp.NegyzetOsszeg);
			TempToReal();
		}

		void SzaldarabEsMaxnegyzetFeltoltese(int[] arr, int hossz, int szalhossz, out int szaldarab, out int negyzetosszeg)
		{
			int ig = hossz - 1, akthull = szalhossz;
			szaldarab = 0; negyzetosszeg = 0;
			for (int i = 0; i <= ig; ++i)
			{
				if (arr[i] > akthull)
				{
					++szaldarab;
					negyzetosszeg += akthull * akthull;
					akthull = szalhossz - arr[i];
				}
				else
				{
					akthull -= arr[i];
				}
			}
			negyzetosszeg += akthull * akthull;
		}

		int[] Optimalizalo()
		{
			//A visszatérési érték.
			int[] retArr = new int[10000];
			int retArrindex = 0;
			//Az eredeti arr() tömb másolása, és csökkenő rendezése:
			int[] arrcpy = new int[cuttingPlanTemp.Eredmeny.Length]; cuttingPlanTemp.Eredmeny.CopyTo(arrcpy, 0);
			Array.Sort(arrcpy); Array.Reverse(arrcpy);
			//Az eredeti arr() tömb tömörítése a tomArr()-ba.
			int[] darabszamok0 = new int[] { };
			int[] tomArr = Tomorito(arrcpy, ref darabszamok0);
			int[] minhullDarabok = new int[] { };
			Array.Resize(ref minhullDarabok, darabszamok0.Length);
			darabszamok0.CopyTo(minhullDarabok, 0);
			int[] temparr;
			//Amíg az eredeti arr() tömb összes darabját nem dolgoztuk fel:
			do
			{
				MinhulladekKombinaciok(ref tomArr, cuttingPlanTemp.Szalhossz, minhullDarabok);
				if (cuttingPlanTemp.KombArrIndex > 0)
				{
					temparr = Kivalasztas(ref tomArr, ref darabszamok0, cuttingPlanTemp.KombArr, cuttingPlanTemp.Szalhossz);
					if (darabszamok0.Length < minhullDarabok.Length) Array.Resize(ref minhullDarabok, darabszamok0.Length);
					darabszamok0.CopyTo(minhullDarabok, 0);
					if (retArr.Length < retArrindex + temparr.Length) Array.Resize(ref retArr, retArr.Length + temparr.Length);
					Array.Copy(temparr, 0, retArr, retArrindex, temparr.Length);
					retArrindex += temparr.Length;
				}
				else
				{
					int ig = tomArr.Length;
					if (ig > 0)
					{
						for (int i = 0; i < ig; ++i)
						{
							if (retArr.Length < retArrindex + minhullDarabok[i]) Array.Resize(ref retArr, retArr.Length + minhullDarabok[i]);
							for (int j = 0; j < minhullDarabok[i]; ++j)
							{
								retArr[retArrindex] = tomArr[i];
								++retArrindex;
							}
						}
					}
				}
			} while (cuttingPlanTemp.KombArrIndex > 0);
			Array.Resize(ref retArr, retArrindex);
			return retArr;
		}

		int[] Optimalizalo2(long probakszama)
		{
			int szaldarab = 0, maxnegyzet = 0;
			int[] hatarok = new int[cuttingPlanTemp.Eredmeny.Length];
			int[] hulladekok = new int[cuttingPlanTemp.Eredmeny.Length];
			int[] darabszamok = new int[cuttingPlanTemp.Eredmeny.Length];
			JellemzokM(ref cuttingPlanTemp.Eredmeny, ref cuttingPlanTemp.Szalhossz, ref szaldarab, ref hatarok, ref hulladekok, ref darabszamok, ref maxnegyzet);
			int szaldb = cuttingPlanTemp.Szaldarab;
			int mn = cuttingPlanTemp.NegyzetOsszeg;
			CuttingStockM(cuttingPlanTemp.Eredmeny, ref hatarok, ref hulladekok, ref darabszamok, ref szaldarab, ref maxnegyzet, cuttingPlanTemp.Szalhossz, 40000);
			JellemzokM(ref cuttingPlanTemp.Eredmeny, ref cuttingPlanTemp.Szalhossz, ref szaldarab, ref hatarok, ref hulladekok, ref darabszamok, ref maxnegyzet);
			long probak = 0;
			do
			{
				CuttingStock(ref cuttingPlanTemp.Eredmeny, ref hatarok, ref hulladekok, ref darabszamok, ref szaldarab, ref maxnegyzet, cuttingPlanTemp.Szalhossz, 20);
				JellemzokHosszra(cuttingPlanTemp.Eredmeny, cuttingPlanTemp.Eredmeny.Length, ref cuttingPlanTemp.Szalhossz, ref szaldarab, hatarok, hulladekok, darabszamok, ref maxnegyzet);
				++probak;
				if (ende) break;
			} while (probak < probakszama);
			JellemzokHosszra(cuttingPlanTemp.Eredmeny, cuttingPlanTemp.Eredmeny.Length, ref cuttingPlanTemp.Szalhossz, ref szaldarab, hatarok, hulladekok, darabszamok, ref maxnegyzet);
			cuttingPlan.Eredmeny = (int[])cuttingPlanTemp.Eredmeny.Clone();
			TempToReal();
			return cuttingPlanTemp.Eredmeny;
		}


		//arr(), darabarr(): a rendelkezésre álló hosszok és azok darabszámai tömörítve.
		//minhullarr(): a minimum hulladékosok tömörítetlenül.
		int[] Kivalasztas(ref int[] arr, ref int[] darabarr, int[] minhullarr, int szalhossz)
		{
			int[] kivArr = new int[] { };
			int nossz = 0, aktindex, szaldarab = 0;
			int[] hatarok = new int[1 + cuttingPlanTemp.KombArrIndex];
			int[] hulladekok = new int[1 + cuttingPlanTemp.KombArrIndex];
			int[] darabszamok = new int[1 + cuttingPlanTemp.KombArrIndex];
			int[] temparr = new int[] { };
			//minhullarr() jellemzőinek meghatározása:
			JellemzokHosszra(minhullarr, cuttingPlanTemp.KombArrIndex, ref szalhossz, ref szaldarab, hatarok, hulladekok, darabszamok, ref nossz);
			//Az utolsó(kiválasztott) sor darabjainak átmásolása a visszatérési értékbe.
			Array.Resize(ref kivArr, darabszamok[szaldarab]);
			Array.Copy(minhullarr, hatarok[szaldarab], kivArr, 0, darabszamok[szaldarab]);
			//Darabszámok csökkentése:
			for (int i = 0; i < kivArr.Length; ++i)
			{
				aktindex = Array.IndexOf(arr, kivArr[i]);
				--darabarr[aktindex];
				if (darabarr[aktindex] == 0)
				{
					//Aktuális darab elfogyott.
					if (arr.Length == 1)
					{
						Array.Resize(ref arr, 0);
						Array.Resize(ref darabarr, 0);
					}
					else
					{
						Array.Copy(arr, aktindex + 1, arr, aktindex, arr.Length - aktindex - 1);
						Array.Copy(darabarr, aktindex + 1, darabarr, aktindex, darabarr.Length - aktindex - 1);
						Array.Resize(ref arr, arr.Length - 1);
						Array.Resize(ref darabarr, darabarr.Length - 1);
					}
				}
			}
			return kivArr;
		}

		void MaxDarabOptimalizalo(int[] arr, int szalhossz, int[] maxindex)
		{
			int ig = arr.Length;
			//tömörített tömbben levő hosszok maximális darabszámát határozza meg a szalhossz függvényében.
			for (int i = 0; i < ig; ++i)
			{
				maxindex[i] = Math.Min(maxindex[i], (szalhossz / arr[i]));
			}
		}

		void MinhulladekKombinaciok(ref int[] arr, int szalhossz, int[] maxindex)
		{
			cuttingPlanTemp.KombArrIndex = 0;
			//if (arr.Length != maxindex.Length) throw new ApplicationException("Exception Occured");
			//Optimalizálja az arr() tömörített tömb darabszámait, majd tömörítetlen tömbbe helyezi, ami a visszatérési érték.
			int[] arr1 = new int[arr.Length]; arr.CopyTo(arr1, 0);
			MaxDarabOptimalizalo(arr1, szalhossz, maxindex);
			int j, k, p = -1, darab = 0, hull = szalhossz, n = maxindex.Length, ig = n, minhull = szalhossz;
			int[] index = new int[n];
			do
			{
				while (p < n - 1)
				{
					++p;
					index[p] = 0; //minindex(p)
				}
				bool sat = true;
				for (j = 0; j < ig; ++j)
				{
					if ((hull - arr1[j]) >= 0)
					{
						sat = false;
						break;
					}
				}

				if (sat)
				{
					++darab;
					if (hull < minhull)
					{
						minhull = hull; cuttingPlanTemp.KombArrIndex = 0;
						for (j = 0; j < index.Length; ++j)
						{
							if (index[j] > 0)
							{
								for (k = 0; k < index[j]; ++k)
								{
									cuttingPlanTemp.KombArr[cuttingPlanTemp.KombArrIndex] = arr1[j];
									++cuttingPlanTemp.KombArrIndex;
								}
							}
						}
					}
					else if (hull == minhull)
					{
						for (j = 0; j < index.Length; ++j)
						{
							if (index[j] > 0)
							{
								for (k = 0; k < index[j]; ++k)
								{
									cuttingPlanTemp.KombArr[cuttingPlanTemp.KombArrIndex] = arr1[j];
									++cuttingPlanTemp.KombArrIndex;
								}
							}
						}
					}
				}

				while (p > -1)
				{
					if (index[p] < maxindex[p])
					{
						++index[p]; hull -= arr1[p];
						if (hull < 0)
						{
							hull += arr1[p]; --index[p];
							hull += arr1[p] * index[p];
							--p;
							continue;
						}
						break;
					}
					else
					{
						hull += arr1[p] * index[p];
						--p;
					}
				}
			} while (p > -1);
		}

		int[] Tomorito(int[] arr, ref int[] darabszamok)
		{
			//arr() tömbben levő elemeket tömöríti úgy, hogy a tom() tömbben lesznek a hosszok,
			//a darabok() tömbben pedig az adott hossz darabszáma.
			//A tom() tömbbel tér vissza.
			int ig = arr.Length, tomindex = 0, arrindex = 0;
			int[] tom = new int[arr.Length];
			darabszamok = new int[arr.Length];
			tom[tomindex] = arr[arrindex];
			while (arrindex < ig)
			{
				if (arr[arrindex] == tom[tomindex]) ++darabszamok[tomindex];
				else
				{
					++tomindex;
					tom[tomindex] = arr[arrindex];
					darabszamok[tomindex] = 1;
				}
				++arrindex;
			}
			Array.Resize(ref tom, 1 + tomindex);
			Array.Resize(ref darabszamok, 1 + tomindex);
			return tom;
		}

		void CuttingStock(ref int[] arr, ref int[] hatarok, ref int[] hulladekok, ref int[] darabszamok, ref int szaldarab, ref int maxnegyzet, int szalhossz, int probakszama)
		{
			int darab = arr.Length;
			JellemzokHosszra(arr, arr.Length, ref szalhossz, ref szaldarab, hatarok, hulladekok, darabszamok, ref maxnegyzet);
			int aktnegyzet = maxnegyzet;
			int[] arr0 = new int[arr.Length];
			arr.CopyTo(arr0, 0);
			int gen0 = 0, gen1 = 0, temp, maxszal = szaldarab;
			//int szal1 = 0, szal2 = 0;
			for (int i = 0; i <= probakszama; ++i)
			{
				for (int k = 0; k < 3; ++k)
				{
					gen0 = rnd.Next(darab);
					gen1 = rnd.Next(darab);
					temp = arr0[gen0];
					arr0[gen0] = arr0[gen1];
					arr0[gen1] = temp;
					JellemzokHosszra(arr0, arr0.Length, ref szalhossz, ref szaldarab, hatarok, hulladekok, darabszamok, ref aktnegyzet);
					if (szaldarab < maxszal) break;
				}
				if (szaldarab < maxszal)
				{
					arr0.CopyTo(arr, 0);
					maxnegyzet = aktnegyzet;
					maxszal = szaldarab;
				}
				else if (szaldarab == maxszal)
				{
					if (aktnegyzet > maxnegyzet)
					{
						maxnegyzet = aktnegyzet;
						maxszal = szaldarab;
						arr0.CopyTo(arr, 0);
					}
				}
			}
		}

		void CuttingStockM(int[] arr, ref int[] hatarok, ref int[] hulladekok, ref int[] darabszamok, ref int szaldarab, ref int maxnegyzet, int szalhossz, int probakszama)
		{
			int darab = arr.Length;
			int aktnegyzet = maxnegyzet;
			int[] arr0 = new int[arr.Length];
			arr.CopyTo(arr0, 0);
			int gen0 = 0, gen1 = 0, temp;
			int maxszal = szaldarab;
			for (int i = 0; i <= probakszama; ++i)
			{
				for (int k = 0; k < 3; ++k)
				{
					int pr0 = 0;
					gen0 = rnd.Next(darab);
					do
					{
						++pr0;
						gen1 = rnd.Next(darab);
					} while (((gen0 == gen1) || (arr0[gen0] == arr0[gen1])) && pr0 < 4);
					temp = arr0[gen0];
					arr0[gen0] = arr0[gen1];
					arr0[gen1] = temp;
				}
				JellemzokM(ref arr0, ref szalhossz, ref szaldarab, ref hatarok, ref hulladekok, ref darabszamok, ref aktnegyzet);
				if (szaldarab < maxszal)
				{
					arr0.CopyTo(arr, 0);
					maxnegyzet = aktnegyzet;
					maxszal = szaldarab;
				}
				else if (szaldarab == maxszal)
				{
					if (aktnegyzet > maxnegyzet)
					{
						maxnegyzet = aktnegyzet;
						maxszal = szaldarab;
						arr0.CopyTo(arr, 0);
					}
				}
				else
				{
					arr.CopyTo(arr0, 0);
				}
			}
		}

		void JellemzokM(ref int[] arr, ref int szalhossz, ref int szaldarab, ref int[] hatarok, ref int[] hulladekok, ref int[] darabszamok, ref int negyzetosszeg)
		{
			int i = 0, ig = arr.Length - 1, maxhull = 0;
			bool nincs = true;
			hulladekok[0] = szalhossz;
			hatarok[0] = 0;
			szaldarab = 0;
			negyzetosszeg = 0;
			int temp = 0, k = 0, ig2;
			int j;
			for (i = 0; i <= ig; ++i)
			{
				nincs = true;
				if (arr[i] <= maxhull)
				{
					ig2 = szaldarab - 1;
					for (j = 0; j <= ig2; ++j)
					{
						if (arr[i] <= hulladekok[j])
						{
							nincs = false;
							negyzetosszeg -= (hulladekok[j] * hulladekok[j]);
							hulladekok[j] -= arr[i];
							negyzetosszeg += (hulladekok[j] * hulladekok[j]);
							temp = arr[i];
							k = i;
							while (k > hatarok[j])
							{
								arr[k] = arr[k - 1];
								--k;
							}
							arr[k] = temp;
							for (k = j + 1; k <= szaldarab; ++k)
							{
								++hatarok[k];
							}
							break;
						}
					}
				}
				if (nincs)
				{
					if (hulladekok[szaldarab] < arr[i])
					{
						negyzetosszeg += (hulladekok[szaldarab] * hulladekok[szaldarab]);
						if (hulladekok[szaldarab] > maxhull) maxhull = hulladekok[szaldarab];
						++szaldarab;
						hulladekok[szaldarab] = szalhossz - arr[i];
						hatarok[szaldarab] = i;
					}
					else
					{
						hulladekok[szaldarab] -= arr[i];
					}
				}
			}
			negyzetosszeg += (hulladekok[szaldarab] * hulladekok[szaldarab]);
			hatarok[szaldarab + 1] = arr.Length;
		}

		void JellemzokHosszra(int[] arr, int hossz, ref int szalhossz, ref int szaldarab, int[] hatarok, int[] hulladekok, int[] darabszamok, ref int negyzetosszeg)
		{
			int ig = hossz - 1;
			hulladekok[0] = szalhossz;
			hatarok[0] = 0;
			darabszamok[0] = 0;
			szaldarab = 0;
			negyzetosszeg = 0;
			for (int i = 0; i <= ig; ++i)
			{
				if (hulladekok[szaldarab] < arr[i])
				{
					negyzetosszeg += (hulladekok[szaldarab] * hulladekok[szaldarab]);
					++szaldarab;
					hulladekok[szaldarab] = szalhossz - arr[i];
					hatarok[szaldarab] = i;
					darabszamok[szaldarab] = 1;
				}
				else
				{
					hulladekok[szaldarab] -= arr[i];
					++darabszamok[szaldarab];
				}
			}
			negyzetosszeg += (hulladekok[szaldarab] * hulladekok[szaldarab]);
			hatarok[1 + szaldarab] = arr.Length;
		}

		int[,] ConvertTomb1dToTomb2d(int[] tomb, int szalhossz)
		{
			int[,] arr = new int[1 + tomb.Length, 2 + tomb.Length];
			//Array.Sort(tomb);
			//Array.Reverse(tomb);
			int aktsor = 0;
			arr[aktsor, 0] = 0;
			arr[aktsor, 1] = szalhossz;
			arr[1 + aktsor, 1] = szalhossz;
			for (int i = 0; i < tomb.Length; ++i)
			{
				if (arr[aktsor, 1] >= tomb[i])
				{
					arr[aktsor, 1] -= tomb[i];
					arr[aktsor, 2 + arr[aktsor, 0]] = tomb[i];
					++arr[aktsor, 0];
				}
				else
				{
					++aktsor;
					arr[aktsor, 1] = szalhossz - tomb[i];
					arr[aktsor, 2] = tomb[i];
					arr[aktsor, 0] = 1;
				}
			}
			arr[1 + aktsor, 1] = szalhossz;
			return arr;
		}

		string Sorstringbe(int[,] arr, int index)
		{
			StringBuilder s = new StringBuilder();
			int jmax = 2 + arr[index, 0];
			for (int j = 2; j < jmax; ++j)
			{
				s.Append(arr[index, j].ToString().PadLeft(4, ' ') + " ");
			}
			s.Append(": " + arr[index, 1].ToString().PadLeft(4, ' '));
			return s.ToString();
		}

		string WString2D()
		{
			if (cuttingPlan.Osszeg == 0) return "";
			cuttingPlanTemp.KettodArr = ConvertTomb1dToTomb2d(cuttingPlanTemp.Eredmeny, cuttingPlanTemp.Szalhossz);
			Rendezesek.RendezHulladekCsokkeno2D(cuttingPlanTemp.KettodArr, 1 + cuttingPlanTemp.Szaldarab);
			Rendezesek.SzalRendez2D(cuttingPlanTemp.KettodArr, 1 + cuttingPlanTemp.Szaldarab);
			StringBuilder s = new StringBuilder();
			int sormax = 1 + cuttingPlanTemp.Szaldarab;
			int osszhull = 0;
			int hullnegyzet = 0;
			int[] indexek = new int[2 + cuttingPlanTemp.Szaldarab];
			int ind = 0;
			indexek[ind] = 0;
			++ind;
			osszhull += cuttingPlanTemp.KettodArr[0, 1];
			hullnegyzet += (cuttingPlanTemp.KettodArr[0, 1] * cuttingPlanTemp.KettodArr[0, 1]);
			for (int i = 1; i < sormax; ++i)
			{
				osszhull += cuttingPlanTemp.KettodArr[i, 1];
				hullnegyzet += (cuttingPlanTemp.KettodArr[i, 1] * cuttingPlanTemp.KettodArr[i, 1]);
				if (cuttingPlanTemp.KettodArr[i, 1] != cuttingPlanTemp.KettodArr[i - 1, 1])
				{
					indexek[ind] = i;
					++ind;
				}
			}
			indexek[ind] = sormax;

			string[] strt = new string[1 + cuttingPlanTemp.Szaldarab];
			int strtind = 0; ;
			for (ind = 0; indexek[ind] < sormax; ++ind)
			{
				int sormaxi = indexek[1 + ind];
				Array.Resize(ref strt, sormaxi - indexek[ind]);
				strtind = 0;
				int i;
				for (i = indexek[ind]; i < sormaxi; ++i)
				{
					strt[strtind] = Sorstringbe(cuttingPlanTemp.KettodArr, i);
					++strtind;
				}
				Array.Sort(strt); Array.Reverse(strt);

				sormaxi = strt.Length;
				int akt = 0;
				string[] erdm = new string[sormaxi];
				int[] db = new int[sormaxi];
				for (i = 0; i < sormaxi; ++i)
				{
					if (erdm[akt] == null)
					{
						erdm[akt] = strt[i];
						db[akt] = 1;
					}
					else
					{
						if (strt[i] == erdm[akt]) ++db[akt];
						else
						{
							++akt;
							erdm[akt] = strt[i];
							db[akt] = 1;
						}
					}
				}
				for (i = 0; i < sormaxi; ++i)
				{
					if (erdm[i] == null) break;
					s.Append(db[i].ToString().PadLeft(3, ' ') + " db " + erdm[i] + Environment.NewLine);
				}
			}

			s.Append(Environment.NewLine + "Összesen " + cuttingPlan.Eredmeny.Length.ToString().PadLeft(4, ' ') + " darab " + (1 + cuttingPlan.Szaldarab).ToString().PadLeft(4, ' ') + " szálban." + Environment.NewLine);
			s.Append("Hulladék: " + osszhull.ToString().PadLeft(4, ' ') + " Összeg: " + cuttingPlan.Osszeg.ToString().PadLeft(4, ' ') + Environment.NewLine);
			s.Append("Négyzetösszeg: " + hullnegyzet.ToString().PadLeft(10, ' ') + Environment.NewLine);
			int min = cuttingPlan.Osszeg / cuttingPlan.Szalhossz;
			if ((cuttingPlan.Osszeg % cuttingPlan.Szalhossz) > 0) ++min;
			s.Append("Elméleti minimum szál: " + min.ToString().PadLeft(4, ' ') + Environment.NewLine);
			return s.ToString();
		}

		public override string ToString()
		{
			return WString2D();
		}
	}

/**************************************************************************************************************************************************************************************************************/

	public class Cutter_2
	{
		CuttingPlan cuttingPlan, cuttingPlanTemp;
		public bool ende = false;
		Random rnd;

		public Cutter_2(string text)
		{
			cuttingPlan = new CuttingPlan();
			cuttingPlanTemp = new CuttingPlan();
			cuttingPlanTemp.KombArr = new int[200000];
			cuttingPlanTemp.Eredmeny = ReadFromString(text, ref cuttingPlanTemp.Szalhossz);
			rnd = new Random(30000);
		}

		static int[] ReadFromString(string text, ref int szalhossz)
		{
			string[] strt2 = text.Split(Environment.NewLine.ToCharArray());
			string[] strt3 = strt2[0].Split(',');
			if (strt3.Length < 2) throw new ApplicationException("Szálhosszt és a ráhagyást is meg kell adni \",\"(vessző)-vel elválasztva!!!");
			if (strt3.Length > 2) throw new ApplicationException("Csak egy szálhosszt és a ráhagyást lehet egy sorban megadni!!!");
			if (!int.TryParse(strt3[0], out szalhossz)) throw new ApplicationException("A szál hossza nem szám!!!");
			int rahagyas = 0;
			if (!int.TryParse(strt3[1], out rahagyas)) throw new ApplicationException("Az ráhagyás nem szám!!!");
			int[] tomb = new int[50];
			int aktindex = 0;
			for (int i = 1; i < strt2.Length; ++i)
			{
				if (strt2[i] != "")
				{
					strt3 = strt2[i].Split(',');
					if (strt3.Length < 2) throw new ApplicationException("Méretet és darabszámot is meg kell adni \",\"(vessző)-vel elválasztva!!!");
					if (strt3.Length > 2) throw new ApplicationException("Csak egy méretet és darabszámot lehet egy sorban megadni!!!");
					int meret, darab;
					if (!int.TryParse(strt3[0], out meret)) throw new ApplicationException("Az egyik méret nem szám!!!");
					meret += rahagyas;
					if (!int.TryParse(strt3[1], out darab)) throw new ApplicationException("Az egyik darabszám nem szám!!!");
					if (aktindex + darab > tomb.Length) Array.Resize(ref tomb, aktindex + darab + 50);
					for (int j = 0; j < darab; ++j) tomb[aktindex++] = meret;
				}
			}
			Array.Resize(ref tomb, aktindex);
			return tomb;
		}

		void TempToReal()
		{
			cuttingPlan.Eredmeny = (int[])cuttingPlanTemp.Eredmeny.Clone();
			cuttingPlan.Szalhossz = cuttingPlanTemp.Szalhossz;
			cuttingPlan.Szaldarab = cuttingPlanTemp.Szaldarab;
			cuttingPlan.Osszeg = cuttingPlanTemp.Osszeg;
			cuttingPlan.NegyzetOsszeg = cuttingPlanTemp.NegyzetOsszeg;
		}

		public void Manipulal(long probakszama)
		{
			int darab = cuttingPlanTemp.Eredmeny.Length;
			cuttingPlanTemp.Osszeg = 0;
			for (int i = 0; i < darab; ++i) cuttingPlanTemp.Osszeg += cuttingPlanTemp.Eredmeny[i];
			cuttingPlanTemp.Darabszamok = new int[1 + cuttingPlanTemp.Eredmeny.Length];
			cuttingPlanTemp.Hatarok = new int[1 + cuttingPlanTemp.Eredmeny.Length];
			cuttingPlanTemp.Hulladekok = new int[1 + cuttingPlanTemp.Eredmeny.Length];
			cuttingPlanTemp.Szaldarab = 0;
			Array.Sort(cuttingPlanTemp.Eredmeny); Array.Reverse(cuttingPlanTemp.Eredmeny);
			JellemzokHosszra(cuttingPlanTemp.Eredmeny, darab, ref cuttingPlanTemp.Szalhossz, ref cuttingPlanTemp.Szaldarab, cuttingPlanTemp.Hatarok, cuttingPlanTemp.Hulladekok, cuttingPlanTemp.Darabszamok, ref cuttingPlanTemp.NegyzetOsszeg);

			cuttingPlanTemp.Eredmeny = Optimalizalo();
			for (int i = 0; i <= cuttingPlanTemp.Szaldarab; ++i)
			{
				Rendezesek.SzalDarabRendezes(cuttingPlanTemp.Eredmeny, cuttingPlanTemp.Hatarok, i, false);
			}

			cuttingPlanTemp.Eredmeny = Optimalizalo11(probakszama);
			SzaldarabEsMaxnegyzetFeltoltese(cuttingPlanTemp.Eredmeny, cuttingPlanTemp.Eredmeny.Length, cuttingPlanTemp.Szalhossz, ref cuttingPlanTemp.Szaldarab, ref cuttingPlanTemp.NegyzetOsszeg);
			TempToReal();
		}

		void SzaldarabEsMaxnegyzetFeltoltese(int[] arr, int hossz, int szalhossz, ref int szaldarab, ref int maxnegyzet)
		{
			int ig = hossz - 1, akthull = szalhossz;
			szaldarab = 0; maxnegyzet = 0;
			for (int i = 0; i <= ig; ++i)
			{
				if (arr[i] > akthull)
				{
					++szaldarab;
					maxnegyzet += akthull * akthull;
					akthull = szalhossz - arr[i];
				}
				else
				{
					akthull -= arr[i];
				}
			}
			maxnegyzet += akthull * akthull;
		}

		int[] Optimalizalo()
		{
			//A visszatérési érték.
			int[] retArr = new int[10000];
			int retArrindex = 0;
			//Az eredeti arr() tömb másolása, és csökkenő rendezése:
			int[] arrcpy = new int[cuttingPlanTemp.Eredmeny.Length]; cuttingPlanTemp.Eredmeny.CopyTo(arrcpy, 0);
			Array.Sort(arrcpy); Array.Reverse(arrcpy);
			//Az eredeti arr() tömb tömörítése a tomArr()-ba.
			int[] darabszamok0 = new int[] { };
			int[] tomArr = Tomorito(arrcpy, ref darabszamok0);
			int[] minhullDarabok = new int[] { };
			Array.Resize(ref minhullDarabok, darabszamok0.Length);
			darabszamok0.CopyTo(minhullDarabok, 0);
			int[] temparr;
			//Amíg az eredeti arr() tömb összes darabját nem dolgoztuk fel:
			do
			{
				MinhulladekKombinaciok(ref tomArr, cuttingPlanTemp.Szalhossz, minhullDarabok);
				if (cuttingPlanTemp.KombArrIndex > 0)
				{
					temparr = Kivalasztas(ref tomArr, ref darabszamok0, cuttingPlanTemp.KombArr, cuttingPlanTemp.Szalhossz);
					if (darabszamok0.Length < minhullDarabok.Length) Array.Resize(ref minhullDarabok, darabszamok0.Length);
					darabszamok0.CopyTo(minhullDarabok, 0);
					if (retArr.Length < retArrindex + temparr.Length) Array.Resize(ref retArr, retArr.Length + temparr.Length);
					Array.Copy(temparr, 0, retArr, retArrindex, temparr.Length);
					retArrindex += temparr.Length;
				}
				else
				{
					int ig = tomArr.Length;
					if (ig > 0)
					{
						for (int i = 0; i < ig; ++i)
						{
							if (retArr.Length < retArrindex + minhullDarabok[i]) Array.Resize(ref retArr, retArr.Length + minhullDarabok[i]);
							for (int j = 0; j < minhullDarabok[i]; ++j)
							{
								retArr[retArrindex] = tomArr[i];
								++retArrindex;
							}
						}
					}
				}
			} while (cuttingPlanTemp.KombArrIndex > 0);
			Array.Resize(ref retArr, retArrindex);
			return retArr;
		}

		int[] Optimalizalo11(long probakszama)
		{
			int szaldarab = 0, maxnegyzet = 0;
			int[] hatarok = new int[cuttingPlanTemp.Eredmeny.Length];
			int[] hulladekok = new int[cuttingPlanTemp.Eredmeny.Length];
			int[] darabszamok = new int[cuttingPlanTemp.Eredmeny.Length];
			JellemzokM(ref cuttingPlanTemp.Eredmeny, ref cuttingPlanTemp.Szalhossz, ref szaldarab, ref hatarok, ref hulladekok, ref darabszamok, ref maxnegyzet);
			int szaldb = cuttingPlanTemp.Szaldarab;
			int mn = cuttingPlanTemp.NegyzetOsszeg;

			CuttingStockM(cuttingPlanTemp.Eredmeny, ref hatarok, ref hulladekok, ref darabszamok, ref szaldarab, ref maxnegyzet, cuttingPlanTemp.Szalhossz, 40000);
			JellemzokM(ref cuttingPlanTemp.Eredmeny, ref cuttingPlanTemp.Szalhossz, ref szaldarab, ref hatarok, ref hulladekok, ref darabszamok, ref maxnegyzet);
			long probak = 0;
			do
			{
				CuttingStock(ref cuttingPlanTemp.Eredmeny, ref hatarok, ref hulladekok, ref darabszamok, ref szaldarab, ref maxnegyzet, cuttingPlanTemp.Szalhossz, 20);
				JellemzokHosszra(cuttingPlanTemp.Eredmeny, cuttingPlanTemp.Eredmeny.Length, ref cuttingPlanTemp.Szalhossz, ref szaldarab, hatarok, hulladekok, darabszamok, ref maxnegyzet);
				++probak;
				if (ende) break;
			} while (probak < probakszama);
			JellemzokHosszra(cuttingPlanTemp.Eredmeny, cuttingPlanTemp.Eredmeny.Length, ref cuttingPlanTemp.Szalhossz, ref szaldarab, hatarok, hulladekok, darabszamok, ref maxnegyzet);
			return cuttingPlanTemp.Eredmeny;
		}

		//arr(), darabarr(): a rendelkezésre álló hosszok és azok darabszámai tömörítve.
		//minhullarr(): a minimum hulladékosok tömörítetlenül.
		int[] Kivalasztas(ref int[] arr, ref int[] darabarr, int[] minhullarr, int szalhossz)
		{
			int[] kivArr = new int[] { };
			int nossz = 0, aktindex, szaldarab = 0;
			int[] hatarok = new int[1 + cuttingPlanTemp.KombArrIndex];
			int[] hulladekok = new int[1 + cuttingPlanTemp.KombArrIndex];
			int[] darabszamok = new int[1 + cuttingPlanTemp.KombArrIndex];
			int[] temparr = new int[] { };
			//minhullarr() jellemzőinek meghatározása:
			JellemzokHosszra(minhullarr, cuttingPlanTemp.KombArrIndex, ref szalhossz, ref szaldarab, hatarok, hulladekok, darabszamok, ref nossz);
			//Az utolsó(kiválasztott) sor darabjainak átmásolása a visszatérési értékbe.
			Array.Resize(ref kivArr, darabszamok[szaldarab]);
			Array.Copy(minhullarr, hatarok[szaldarab], kivArr, 0, darabszamok[szaldarab]);
			//Darabszámok csökkentése:
			for (int i = 0; i < kivArr.Length; ++i)
			{
				aktindex = Array.IndexOf(arr, kivArr[i]);
				--darabarr[aktindex];
				if (darabarr[aktindex] == 0)
				{
					//Aktuális darab elfogyott.
					if (arr.Length == 1)
					{
						Array.Resize(ref arr, 0);
						Array.Resize(ref darabarr, 0);
					}
					else
					{
						Array.Copy(arr, aktindex + 1, arr, aktindex, arr.Length - aktindex - 1);
						Array.Copy(darabarr, aktindex + 1, darabarr, aktindex, darabarr.Length - aktindex - 1);
						Array.Resize(ref arr, arr.Length - 1);
						Array.Resize(ref darabarr, darabarr.Length - 1);
					}
				}
			}
			return kivArr;
		}

		void MaxDarabOptimalizalo(int[] arr, int szalhossz, int[] maxindex)
		{
			int ig = arr.Length;
			//tömörített tömbben levő hosszok maximális darabszámát határozza meg a szalhossz függvényében.
			for (int i = 0; i < ig; ++i)
			{
				maxindex[i] = Math.Min(maxindex[i], (szalhossz / arr[i]));
			}
		}

		void MinhulladekKombinaciok(ref int[] arr, int szalhossz, int[] maxindex)
		{
			cuttingPlanTemp.KombArrIndex = 0;
			//if (arr.Length != maxindex.Length) throw new ApplicationException("Exception Occured");
			//Optimalizálja az arr() tömörített tömb darabszámait, majd tömörítetlen tömbbe helyezi, ami a visszatérési érték.
			int[] arr1 = new int[arr.Length]; arr.CopyTo(arr1, 0);
			MaxDarabOptimalizalo(arr1, szalhossz, maxindex);
			int j, k, p = -1, darab = 0, hull = szalhossz, n = maxindex.Length, ig = n, minhull = szalhossz;
			int[] index = new int[n];
			do
			{
				while (p < n - 1)
				{
					++p;
					index[p] = 0; //minindex(p)
				}
				bool sat = true;
				for (j = 0; j < ig; ++j)
				{
					if ((hull - arr1[j]) >= 0)
					{
						sat = false;
						break;
					}
				}

				if (sat)
				{
					++darab;
					if (hull < minhull)
					{
						minhull = hull; cuttingPlanTemp.KombArrIndex = 0;
						for (j = 0; j < index.Length; ++j)
						{
							if (index[j] > 0)
							{
								for (k = 0; k < index[j]; ++k)
								{
									cuttingPlanTemp.KombArr[cuttingPlanTemp.KombArrIndex] = arr1[j];
									++cuttingPlanTemp.KombArrIndex;
								}
							}
						}
					}
					else if (hull == minhull)
					{
						for (j = 0; j < index.Length; ++j)
						{
							if (index[j] > 0)
							{
								for (k = 0; k < index[j]; ++k)
								{
									cuttingPlanTemp.KombArr[cuttingPlanTemp.KombArrIndex] = arr1[j];
									++cuttingPlanTemp.KombArrIndex;
								}
							}
						}
					}
				}

				while (p > -1)
				{
					if (index[p] < maxindex[p])
					{
						++index[p]; hull -= arr1[p];
						if (hull < 0)
						{
							hull += arr1[p]; --index[p];
							hull += arr1[p] * index[p];
							--p;
							continue;
						}
						break;
					}
					else
					{
						hull += arr1[p] * index[p];
						--p;
					}
				}
			} while (p > -1);
		}

		int[] Tomorito(int[] arr, ref int[] darabszamok)
		{
			//arr() tömbben levő elemeket tömöríti úgy, hogy a tom() tömbben lesznek a hosszok,
			//a darabok() tömbben pedig az adott hossz darabszáma.
			//A tom() tömbbel tér vissza.
			int ig = arr.Length, tomindex = 0, arrindex = 0;
			int[] tom = new int[arr.Length];
			darabszamok = new int[arr.Length];
			tom[tomindex] = arr[arrindex];
			while (arrindex < ig)
			{
				if (arr[arrindex] == tom[tomindex]) ++darabszamok[tomindex];
				else
				{
					++tomindex;
					tom[tomindex] = arr[arrindex];
					darabszamok[tomindex] = 1;
				}
				++arrindex;
			}
			Array.Resize(ref tom, 1 + tomindex);
			Array.Resize(ref darabszamok, 1 + tomindex);
			return tom;
		}

		void CuttingStock(ref int[] arr, ref int[] hatarok, ref int[] hulladekok, ref int[] darabszamok, ref int szaldarab, ref int maxnegyzet, int szalhossz, int probakszama)
		{
			int darab = arr.Length;
			JellemzokHosszra(arr, arr.Length, ref szalhossz, ref szaldarab, hatarok, hulladekok, darabszamok, ref maxnegyzet);
			int aktnegyzet = maxnegyzet;
			int[] arr0 = new int[arr.Length];
			arr.CopyTo(arr0, 0);
			int gen0 = 0, gen1 = 0, temp, szal1 = 0, szal2 = 0, maxszal = szaldarab;
			for (int i = 0; i <= probakszama; ++i)
			{
				for (int k = 0; k < 3; ++k)
				{
					int m = 0;
					do
					{
						gen0 = rnd.Next(darab);
						szal1 = IndexMelySzalban(ref hatarok, gen0);
						gen1 = rnd.Next(darab);
						szal2 = IndexMelySzalban(ref hatarok, gen1);
						++m;
					} while ((szal1 == szal2) || (gen0 == gen1) || (arr0[gen0] == arr0[gen1]) || (hulladekok[szal1] + arr0[gen0] < arr0[gen1]) || (hulladekok[szal2] + arr0[gen1] < arr0[gen0]) && (m < 100));
					temp = arr0[gen0];
					arr0[gen0] = arr0[gen1];
					arr0[gen1] = temp;
					JellemzokHosszra(arr0, arr0.Length, ref szalhossz, ref szaldarab, hatarok, hulladekok, darabszamok, ref aktnegyzet);
					if (szaldarab < maxszal) break;
				}
				if (szaldarab < maxszal)
				{
					arr0.CopyTo(arr, 0);
					maxnegyzet = aktnegyzet;
					maxszal = szaldarab;
				}
				else if (szaldarab == maxszal)
				{
					if (aktnegyzet > maxnegyzet)
					{
						maxnegyzet = aktnegyzet;
						arr0.CopyTo(arr, 0);
					}
				}
			}
		}

		int IndexMelySzalban(ref int[] hatarok, int index)
		{
			int i = 0;
			while ((index >= hatarok[1 + i]) || (index < hatarok[i]))
			{
				++i;
			}
			return i;
		}

		void CuttingStockM(int[] arr, ref int[] hatarok, ref int[] hulladekok, ref int[] darabszamok, ref int szaldarab, ref int maxnegyzet, int szalhossz, int probakszama)
		{
			int darab = arr.Length;
			//Jellemzok(arr, szalhossz, szaldarab, hatarok, hulladekok, darabok)
			//maxnegyzet = Negyzetosszeg(hulladekok, szaldarab)
			int aktnegyzet = maxnegyzet;
			int[] arr0 = new int[arr.Length];
			arr.CopyTo(arr0, 0);
			//CopyMemory(arr0, arr, (uint)(darab * intSize));
			int gen0 = 0, gen1 = 0, temp;
			int maxszal = szaldarab;
			for (int i = 0; i <= probakszama; ++i)
			{
				for (int k = 0; k < 3; ++k)
				{
					gen0 = rnd.Next(darab);
					int m = 0;
					do
					{
						gen1 = rnd.Next(darab);
						++m;
					} while ((gen0 == gen1) || (arr0[gen0] == arr0[gen1]) && (m < 100));
					temp = arr0[gen0];
					arr0[gen0] = arr0[gen1];
					arr0[gen1] = temp;
				}
				JellemzokM(ref arr0, ref szalhossz, ref szaldarab, ref hatarok, ref hulladekok, ref darabszamok, ref aktnegyzet);
				if (szaldarab < maxszal)
				{
					arr0.CopyTo(arr, 0);
					maxnegyzet = aktnegyzet;
					maxszal = szaldarab;
				}
				else if (szaldarab == maxszal)
				{
					if (aktnegyzet > maxnegyzet)
					{
						maxnegyzet = aktnegyzet;
						arr0.CopyTo(arr, 0);
					}
				}
				else
				{
					arr.CopyTo(arr0, 0);
				}
			}
		}

		void JellemzokM(ref int[] arr, ref int szalhossz, ref int szaldarab, ref int[] hatarok, ref int[] hulladekok, ref int[] darabszamok, ref int negyzetosszeg)
		{
			int i = 0, ig = arr.Length - 1, maxhull = 0;
			bool nincs = true;
			hulladekok[0] = szalhossz;
			hatarok[0] = 0;
			szaldarab = 0;
			negyzetosszeg = 0;
			int temp = 0, k = 0, ig2;
			int j;
			for (i = 0; i <= ig; ++i)
			{
				nincs = true;
				if (arr[i] <= maxhull)
				{
					ig2 = szaldarab - 1;
					for (j = 0; j <= ig2; ++j)
					{
						if (arr[i] <= hulladekok[j])
						{
							nincs = false;
							negyzetosszeg -= (hulladekok[j] * hulladekok[j]);
							hulladekok[j] -= arr[i];
							negyzetosszeg += (hulladekok[j] * hulladekok[j]);
							temp = arr[i];
							k = i;
							while (k > hatarok[j])
							{
								arr[k] = arr[k - 1];
								--k;
							}
							arr[k] = temp;
							for (k = j + 1; k <= szaldarab; ++k)
							{
								++hatarok[k];
							}
							break;
						}
					}
				}
				if (nincs)
				{
					if (hulladekok[szaldarab] < arr[i])
					{
						negyzetosszeg += (hulladekok[szaldarab] * hulladekok[szaldarab]);
						if (hulladekok[szaldarab] > maxhull) maxhull = hulladekok[szaldarab];
						++szaldarab;
						hulladekok[szaldarab] = szalhossz - arr[i];
						hatarok[szaldarab] = i;
					}
					else
					{
						hulladekok[szaldarab] -= arr[i];
					}
				}
			}
			negyzetosszeg += (hulladekok[szaldarab] * hulladekok[szaldarab]);
			hatarok[szaldarab + 1] = arr.Length;
		}

		void JellemzokHosszra(int[] arr, int hossz, ref int szalhossz, ref int szaldarab, int[] hatarok, int[] hulladekok, int[] darabszamok, ref int negyzetosszeg)
		{
			int ig = hossz - 1;
			hulladekok[0] = szalhossz;
			hatarok[0] = 0;
			darabszamok[0] = 0;
			szaldarab = 0;
			negyzetosszeg = 0;
			for (int i = 0; i <= ig; ++i)
			{
				if (hulladekok[szaldarab] < arr[i])
				{
					negyzetosszeg += (hulladekok[szaldarab] * hulladekok[szaldarab]);
					++szaldarab;
					hulladekok[szaldarab] = szalhossz - arr[i];
					hatarok[szaldarab] = i;
					darabszamok[szaldarab] = 1;
				}
				else
				{
					hulladekok[szaldarab] -= arr[i];
					++darabszamok[szaldarab];
				}
			}
			negyzetosszeg += (hulladekok[szaldarab] * hulladekok[szaldarab]);
			hatarok[1 + szaldarab] = arr.Length;
		}

		int[,] ConvertTomb1dToTomb2d(int[] tomb, int szalhossz)
		{
			int[,] arr = new int[1 + tomb.Length, 2 + tomb.Length];
			//Array.Sort(tomb);
			//Array.Reverse(tomb);
			int aktsor = 0;
			arr[aktsor, 0] = 0;
			arr[aktsor, 1] = szalhossz;
			arr[1 + aktsor, 1] = szalhossz;
			for (int i = 0; i < tomb.Length; ++i)
			{
				if (arr[aktsor, 1] >= tomb[i])
				{
					arr[aktsor, 1] -= tomb[i];
					arr[aktsor, 2 + arr[aktsor, 0]] = tomb[i];
					++arr[aktsor, 0];
				}
				else
				{
					++aktsor;
					arr[aktsor, 1] = szalhossz - tomb[i];
					arr[aktsor, 2] = tomb[i];
					arr[aktsor, 0] = 1;
				}
			}
			arr[1 + aktsor, 1] = szalhossz;
			return arr;
		}

		string Sorstringbe(int[,] arr, int index)
		{
			StringBuilder s = new StringBuilder();
			int jmax = 2 + arr[index, 0];
			for (int j = 2; j < jmax; ++j)
			{
				s.Append(arr[index, j].ToString().PadLeft(4, ' ') + " ");
			}
			s.Append(": " + arr[index, 1].ToString().PadLeft(4, ' '));
			return s.ToString();
		}

		string WString2D()
		{
			if (cuttingPlan.Osszeg == 0) return "";
			cuttingPlanTemp.KettodArr = ConvertTomb1dToTomb2d(cuttingPlanTemp.Eredmeny, cuttingPlanTemp.Szalhossz);
			Rendezesek.RendezHulladekCsokkeno2D(cuttingPlanTemp.KettodArr, 1 + cuttingPlanTemp.Szaldarab);
			Rendezesek.SzalRendez2D(cuttingPlanTemp.KettodArr, 1 + cuttingPlanTemp.Szaldarab);
			StringBuilder s = new StringBuilder();
			int sormax = 1 + cuttingPlanTemp.Szaldarab;
			int osszhull = 0;
			int hullnegyzet = 0;
			int[] indexek = new int[2 + cuttingPlanTemp.Szaldarab];
			int ind = 0;
			indexek[ind] = 0;
			++ind;
			osszhull += cuttingPlanTemp.KettodArr[0, 1];
			hullnegyzet += (cuttingPlanTemp.KettodArr[0, 1] * cuttingPlanTemp.KettodArr[0, 1]);
			for (int i = 1; i < sormax; ++i)
			{
				osszhull += cuttingPlanTemp.KettodArr[i, 1];
				hullnegyzet += (cuttingPlanTemp.KettodArr[i, 1] * cuttingPlanTemp.KettodArr[i, 1]);
				if (cuttingPlanTemp.KettodArr[i, 1] != cuttingPlanTemp.KettodArr[i - 1, 1])
				{
					indexek[ind] = i;
					++ind;
				}
			}
			indexek[ind] = sormax;

			string[] strt = new string[1 + cuttingPlanTemp.Szaldarab];
			int strtind = 0; ;
			for (ind = 0; indexek[ind] < sormax; ++ind)
			{
				int sormaxi = indexek[1 + ind];
				Array.Resize(ref strt, sormaxi - indexek[ind]);
				strtind = 0;
				int i;
				for (i = indexek[ind]; i < sormaxi; ++i)
				{
					strt[strtind] = Sorstringbe(cuttingPlanTemp.KettodArr, i);
					++strtind;
				}
				Array.Sort(strt); Array.Reverse(strt);

				sormaxi = strt.Length;
				int akt = 0;
				string[] erdm = new string[sormaxi];
				int[] db = new int[sormaxi];
				for (i = 0; i < sormaxi; ++i)
				{
					if (erdm[akt] == null)
					{
						erdm[akt] = strt[i];
						db[akt] = 1;
					}
					else
					{
						if (strt[i] == erdm[akt]) ++db[akt];
						else
						{
							++akt;
							erdm[akt] = strt[i];
							db[akt] = 1;
						}
					}
				}
				for (i = 0; i < sormaxi; ++i)
				{
					if (erdm[i] == null) break;
					s.Append(db[i].ToString().PadLeft(3, ' ') + " db " + erdm[i] + Environment.NewLine);
				}
			}

			s.Append(Environment.NewLine + "Összesen " + cuttingPlan.Eredmeny.Length.ToString().PadLeft(4, ' ') + " darab " + (1 + cuttingPlanTemp.Szaldarab).ToString().PadLeft(4, ' ') + " szálban." + Environment.NewLine);
			s.Append("Hulladék: " + osszhull.ToString().PadLeft(4, ' ') + " Összeg: " + cuttingPlan.Osszeg.ToString().PadLeft(4, ' ') + Environment.NewLine);
			s.Append("Négyzetösszeg: " + hullnegyzet.ToString().PadLeft(10, ' ') + Environment.NewLine);
			int min = cuttingPlan.Osszeg / cuttingPlan.Szalhossz;
			if ((cuttingPlan.Osszeg % cuttingPlan.Szalhossz) > 0) ++min;
			s.Append("Elméleti minimum szál: " + min.ToString().PadLeft(4, ' ') + Environment.NewLine);
			return s.ToString();
		}

		public override string ToString()
		{
			return WString2D();
		}
	}


	/******************************************************************************************************************************************************************************************************/


	public class Cutter_3
	{
		[DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
		public static extern void CopyMemory(int[] dest, int[] src, uint count);
		static int intSize = Marshal.SizeOf(typeof(int));

		public bool ende = false;

		int[] forras;
		int szalhossz;
		int[] Eredmeny;
		int[,] KettodArr;
		int Szaldarab, Maxnegyzet, Osszeg;

		public Cutter_3(string text)
		{
			forras = ReadFromString(text, ref szalhossz);
			Array.Sort(forras);
			//hosszok = Tomorito(forras, ref darabszamok);
			//Console.WriteLine(IsmPermutacioDarab(forras.Length, darabszamok.Length, darabszamok));
		}

		public void Manipulal()
		{
			int n = forras.Length;
			Eredmeny = new int[n];
			Array.Copy(forras, Eredmeny, n);
			SzaldarabEsMaxnegyzetFeltoltese(forras, n, szalhossz, ref Szaldarab, ref Maxnegyzet);
			Teszt_1(forras);
			SzaldarabEsMaxnegyzetFeltoltese(Eredmeny, Eredmeny.Length, szalhossz, ref Szaldarab, ref Maxnegyzet);
			int darab = Eredmeny.Length;
			Osszeg = 0;
			for (int i = 0; i < darab; ++i) Osszeg += Eredmeny[i];
		}

		void Teszt_1(int[] tomb)
		{
			int size = tomb.Length;
			Rendezesek.QuickSort(tomb, 0, size - 1);
			//Array.Sort(tomb);
			int n = tomb.Length;
			int i, j, temp;
			uint mashossz = (uint)(size * intSize);
			while (true)
			{
				int szdb = 0, maxn = 0;
				SzaldarabEsMaxnegyzetFeltoltese(tomb, size, szalhossz, ref szdb, ref maxn);
				if (szdb < Szaldarab)
				{
					CopyMemory(Eredmeny, tomb, mashossz);
					Szaldarab = szdb;
					Maxnegyzet = maxn;
				}
				else if (szdb == Szaldarab)
				{
					if (maxn > Maxnegyzet)
					{
						CopyMemory(Eredmeny, tomb, mashossz);
						Szaldarab = szdb;
						Maxnegyzet = maxn;
					}
				}
				if (ende) break;
				// kiirjuk az aktualis permutaciot
				/*for (i = 0; i < n; ++i) Console.Write("{0} ", arr2[i]);
                Console.WriteLine("");*/

				// megkeressuk, hol kezdodik az utolso monoton csokkeno reszsorozat
				for (i = n - 2; i >= 0 && tomb[i] >= tomb[i + 1]; --i) ;
				// ha a teljes sorozat monoton csokkeno, akkor vegeztunk
				if (i < 0) break;
				// a csokkeno reszsorozat elotti elemet ki kell cserelnunk a reszsorozatban nagysag szerint rakovetkezovel
				for (j = n - 1; tomb[j] <= tomb[i]; --j) ;
				temp = tomb[i]; tomb[i] = tomb[j]; tomb[j] = temp;
				// tovabbra is monoton csokkeno a reszsorozatunk, forditsuk meg, hogy monoton novekedo legyen
				for (j = i + 1; j < n + i - j; ++j)
				{
					temp = tomb[j]; tomb[j] = tomb[n + i - j]; tomb[n + i - j] = temp;
				}
			}

		}


		void SzaldarabEsMaxnegyzetFeltoltese(int[] arr, int hossz, int szalhossz, ref int szaldarab, ref int maxnegyzet)
		{
			int ig = hossz - 1, akthull = szalhossz;
			maxnegyzet = 0;
			for (int i = 0; i <= ig; ++i)
			{
				if (arr[i] > akthull)
				{
					++szaldarab;
					maxnegyzet += akthull * akthull;
					akthull = szalhossz - arr[i];
				}
				else
				{
					akthull -= arr[i];
				}
			}
			maxnegyzet += akthull * akthull;
		}

		static int[] ReadFromString(string text, ref int szalhossz)
		{
			string[] strt2 = text.Split(Environment.NewLine.ToCharArray());
			string[] strt3 = strt2[0].Split(',');
			if (strt3.Length < 2) throw new ApplicationException("Szálhosszt és a ráhagyást is meg kell adni \",\"(vessző)-vel elválasztva!!!");
			if (strt3.Length > 2) throw new ApplicationException("Csak egy szálhosszt és a ráhagyást lehet egy sorban megadni!!!");
			if (!int.TryParse(strt3[0], out szalhossz)) throw new ApplicationException("A szál hossza nem szám!!!");
			int rahagyas = 0;
			if (!int.TryParse(strt3[1], out rahagyas)) throw new ApplicationException("Az ráhagyás nem szám!!!");
			int[] tomb = new int[50];
			int aktindex = 0;
			for (int i = 1; i < strt2.Length; ++i)
			{
				if (strt2[i] != "")
				{
					strt3 = strt2[i].Split(',');
					if (strt3.Length < 2) throw new ApplicationException("Méretet és darabszámot is meg kell adni \",\"(vessző)-vel elválasztva!!!");
					if (strt3.Length > 2) throw new ApplicationException("Csak egy méretet és darabszámot lehet egy sorban megadni!!!");
					int meret, darab;
					if (!int.TryParse(strt3[0], out meret)) throw new ApplicationException("Az egyik méret nem szám!!!");
					meret += rahagyas;
					if (!int.TryParse(strt3[1], out darab)) throw new ApplicationException("Az egyik darabszám nem szám!!!");
					if (aktindex + darab > tomb.Length) Array.Resize(ref tomb, aktindex + darab + 50);
					for (int j = 0; j < darab; ++j) tomb[aktindex++] = meret;
				}
			}
			Array.Resize(ref tomb, aktindex);
			return tomb;
		}
		
		static long IsmPermutacioDarab(int n, int n1, int[] k)
		{
			long temp = 1;
			for (int j = 0; j < n1; ++j)
			{
				for (int i = 0; i <= k[j] - 1; ++i)
				{
					temp = temp * n / (i + 1);
					--n;
				}
			}
			return temp;
		}
		

		int[,] ConvertTomb1dToTomb2d(int[] tomb, int szalhossz)
		{
			int[,] arr = new int[1 + tomb.Length, 2 + tomb.Length];
			//Array.Sort(tomb);
			//Array.Reverse(tomb);
			int aktsor = 0;
			arr[aktsor, 0] = 0;
			arr[aktsor, 1] = szalhossz;
			arr[1 + aktsor, 1] = szalhossz;
			for (int i = 0; i < tomb.Length; ++i)
			{
				if (arr[aktsor, 1] >= tomb[i])
				{
					arr[aktsor, 1] -= tomb[i];
					arr[aktsor, 2 + arr[aktsor, 0]] = tomb[i];
					++arr[aktsor, 0];
				}
				else
				{
					++aktsor;
					arr[aktsor, 1] = szalhossz - tomb[i];
					arr[aktsor, 2] = tomb[i];
					arr[aktsor, 0] = 1;
				}
			}
			arr[1 + aktsor, 1] = szalhossz;
			Szaldarab = aktsor;
			return arr;
		}

		string Sorstringbe(int[,] arr, int index)
		{
			StringBuilder s = new StringBuilder();
			int jmax = 2 + arr[index, 0];
			for (int j = 2; j < jmax; ++j)
			{
				s.Append(arr[index, j].ToString().PadLeft(4, ' ') + " ");
			}
			s.Append(": " + arr[index, 1].ToString().PadLeft(4, ' '));
			return s.ToString();
		}

		string SorstringbeCsv(int[,] arr, int index)
		{
			StringBuilder s = new StringBuilder();
			int jmax = 2 + arr[index, 0];
			for (int j = 2; j < jmax; ++j)
			{
				s.Append(arr[index, j].ToString().PadLeft(4, ' ') + ";");
			}
			s.Append(":;" + arr[index, 1].ToString().PadLeft(4, ' '));
			return s.ToString();
		}

		public string ToCsv()
		{
			if (Osszeg == 0) return "";
			KettodArr = ConvertTomb1dToTomb2d(Eredmeny, szalhossz);
			Rendezesek.RendezHulladekCsokkeno2D(KettodArr, 1 + Szaldarab);
			Rendezesek.SzalRendez2D(KettodArr, 1 + Szaldarab);
			StringBuilder s = new StringBuilder();
			int sormax = 1 + Szaldarab;
			int osszhull = 0;
			int hullnegyzet = 0;
			int[] indexek = new int[2 + Szaldarab];
			int ind = 0;
			indexek[ind] = 0;
			++ind;
			osszhull += KettodArr[0, 1];
			hullnegyzet += (KettodArr[0, 1] * KettodArr[0, 1]);
			for (int i = 1; i < sormax; ++i)
			{
				osszhull += KettodArr[i, 1];
				hullnegyzet += (KettodArr[i, 1] * KettodArr[i, 1]);
				if (KettodArr[i, 1] != KettodArr[i - 1, 1])
				{
					indexek[ind] = i;
					++ind;
				}
			}
			indexek[ind] = sormax;
			string[] strt = new string[1 + Szaldarab];
			int strtind = 0; ;
			for (ind = 0; indexek[ind] < sormax; ++ind)
			{
				int sormaxi = indexek[1 + ind];
				Array.Resize(ref strt, sormaxi - indexek[ind]);
				strtind = 0;
				int i;
				for (i = indexek[ind]; i < sormaxi; ++i)
				{
					strt[strtind] = SorstringbeCsv(KettodArr, i);
					++strtind;
				}
				Array.Sort(strt); Array.Reverse(strt);

				sormaxi = strt.Length;
				int akt = 0;
				string[] erdm = new string[sormaxi];
				int[] db = new int[sormaxi];
				for (i = 0; i < sormaxi; ++i)
				{
					if (erdm[akt] == null)
					{
						erdm[akt] = strt[i];
						db[akt] = 1;
					}
					else
					{
						if (strt[i] == erdm[akt]) ++db[akt];
						else
						{
							++akt;
							erdm[akt] = strt[i];
							db[akt] = 1;
						}
					}
				}
				for (i = 0; i < sormaxi; ++i)
				{
					if (erdm[i] == null) break;
					s.Append(db[i].ToString().PadLeft(3, ' ') + "; db ;" + erdm[i] + Environment.NewLine);
				}
			}
			return s.ToString();
		}

		string WString2D()
		{
			if (Osszeg == 0) return "";
			KettodArr = ConvertTomb1dToTomb2d(Eredmeny, szalhossz);
			Rendezesek.RendezHulladekCsokkeno2D(KettodArr, 1 + Szaldarab);
			Rendezesek.SzalRendez2D(KettodArr, 1 + Szaldarab);
			StringBuilder s = new StringBuilder();
			int sormax = 1 + Szaldarab;
			int osszhull = 0;
			int hullnegyzet = 0;
			int[] indexek = new int[2 + Szaldarab];
			int ind = 0;
			indexek[ind] = 0;
			++ind;
			osszhull += KettodArr[0, 1];
			hullnegyzet += (KettodArr[0, 1] * KettodArr[0, 1]);
			for (int i = 1; i < sormax; ++i)
			{
				osszhull += KettodArr[i, 1];
				hullnegyzet += (KettodArr[i, 1] * KettodArr[i, 1]);
				if (KettodArr[i, 1] != KettodArr[i - 1, 1])
				{
					indexek[ind] = i;
					++ind;
				}
			}
			indexek[ind] = sormax;

			string[] strt = new string[1 + Szaldarab];
			int strtind = 0; ;
			for (ind = 0; indexek[ind] < sormax; ++ind)
			{
				int sormaxi = indexek[1 + ind];
				Array.Resize(ref strt, sormaxi - indexek[ind]);
				strtind = 0;
				int i;
				for (i = indexek[ind]; i < sormaxi; ++i)
				{
					strt[strtind] = Sorstringbe(KettodArr, i);
					++strtind;
				}
				Array.Sort(strt); Array.Reverse(strt);

				sormaxi = strt.Length;
				int akt = 0;
				string[] erdm = new string[sormaxi];
				int[] db = new int[sormaxi];
				for (i = 0; i < sormaxi; ++i)
				{
					if (erdm[akt] == null)
					{
						erdm[akt] = strt[i];
						db[akt] = 1;
					}
					else
					{
						if (strt[i] == erdm[akt]) ++db[akt];
						else
						{
							++akt;
							erdm[akt] = strt[i];
							db[akt] = 1;
						}
					}
				}
				for (i = 0; i < sormaxi; ++i)
				{
					if (erdm[i] == null) break;
					s.Append(db[i].ToString().PadLeft(3, ' ') + " db " + erdm[i] + Environment.NewLine);
				}
			}

			s.Append(Environment.NewLine + "Összesen " + Eredmeny.Length.ToString().PadLeft(4, ' ') + " darab " + (1 + Szaldarab).ToString().PadLeft(4, ' ') + " szálban." + Environment.NewLine);
			s.Append("Hulladék: " + osszhull.ToString().PadLeft(4, ' ') + " Összeg: " + Osszeg.ToString().PadLeft(4, ' ') + Environment.NewLine);
			s.Append("Négyzetösszeg: " + hullnegyzet.ToString().PadLeft(10, ' ') + Environment.NewLine);
			int min = Osszeg / szalhossz;
			if ((Osszeg % szalhossz) > 0) ++min;
			s.Append("Elméleti minimum szál: " + min.ToString().PadLeft(4, ' ') + Environment.NewLine);
			return s.ToString();
		}

		public override string ToString()
		{
			return WString2D();
		}
	}
}
