using System;
using System.Collections;
using System.IO;

namespace Simulation_Impots
{

    public class Employe
    {
        private string nom, prenom, matricule;
        private int conjoint, nbreEnfant, salaireBrut, nbreJours; 

        public Employe(string eNom, string ePrenom, string eMatricule, int eConjoint, int eNbreEnfant, int eSalaireBrut, int eNbreJours)
        {
            this.nom = eNom;
            this.prenom = ePrenom;
            this.matricule = eMatricule;
            this.conjoint = eConjoint;
            this.nbreEnfant = eNbreEnfant;
            this.salaireBrut = eSalaireBrut;
            this.nbreJours = eNbreJours;
        }

        public Employe()
        {
        }
       
        public string getNom()
        {
            return this.nom;
        }

        public void setNom(string nom)
        {
            this.nom = nom;
        }

        public string getPrenom()
        {
            return this.prenom;
        }

        public void setPrenom(string prenom)
        {
            this.prenom = prenom;
        }

        public string getMatricule()
        {
            return this.matricule;
        }

        public void setMatricule(string matricule)
        {
            this.matricule = matricule;
        }

        public int getConjoint()
        {
            return this.conjoint;
        }

        public void setConjoint(int conjoint)
        {
            this.conjoint = conjoint;
        }

        public int getNbreEnfant()
        {
            return this.nbreEnfant;
        }

        public void setNbreEnfant(int nbreEnfant)
        {
            this.nbreEnfant = nbreEnfant;
        }

        public int getSalaireBrut()
        {
            return this.salaireBrut;
        }

        public void setsalaireBrut(int salaireBrut)
        {
            this.salaireBrut = salaireBrut;
        }

        public int getNbreJours()
        {
            return this.nbreJours;
        }

        public void setNbreJours(int nbreJours)
        {
            this.nbreJours = nbreJours;
        }


        enum Conjoint
        {
            Célibataire,
            ConjointNonSalarié
        }

        public int CalculBrutFiscAnnuel(int salaire, int nbreJours)
        {
            return (salaire/nbreJours)*360;
        }

        public int CalculBrutFiscApresAbattement(int salaireBrut, int abattement)
        {
            return salaireBrut*12 - abattement;
        }

        public double CalculIRPP(int salaire)
        {

            int [] max = new int [] {1500000, 4000000, 8000000, 13500000, 1000000000};
            int[] min = new int[] {630001, 1500001, 4000001, 8000001, 13500001};
            int[] pourcentage = new int[] { 20, 30, 35, 37, 40 };
            int salaireAnnuel = salaire * 12;
            int IRPP = 0;

            if(salaireAnnuel <= min[0])
            {
                IRPP = 0;
            }
            else if (salaireAnnuel > min[0] && salaireAnnuel <= max[0])
            {
                IRPP += (salaireAnnuel - min[0]) * pourcentage[0]/100;
            }
            else if(salaireAnnuel > min[1] && salaireAnnuel <= max[1])
            {
                IRPP += (max[0] - min[0]) * pourcentage[0] / 100 + (salaireAnnuel - min[1]) * pourcentage[1] / 100;
            }
            else if (salaireAnnuel > min[2] && salaireAnnuel <= max[2])
            {
                IRPP += (max[0] - min[0]) * pourcentage[0] / 100 + (max[1] - min[1]) * pourcentage[1] / 100
                        + (salaireAnnuel - min[2]) * pourcentage[2] / 100;
            }
            else if (salaireAnnuel > min[3] && salaireAnnuel <= max[3])
            {
                IRPP += (max[0] - min[0]) * pourcentage[0] / 100 + (max[1] - min[1]) * pourcentage[1] / 100
                        + (max[2] - min[2]) * pourcentage[2] / 100 + (salaireAnnuel - min[3]) * pourcentage[3] / 100;
            }
            else if (salaireAnnuel > min[4] && salaireAnnuel <= max[4])
            {
                IRPP += (max[0] - min[0]) * pourcentage[0] / 100 + (max[1] - min[1]) * pourcentage[1] / 100
                        + (max[2] - min[2]) * pourcentage[2] / 100 + (max[3] - min[3]) * pourcentage[3] / 100
                        + (salaireAnnuel - min[4]) * pourcentage[4] / 100;
            }

            return IRPP;

        }

        public int CalculAbattement(int salaire)
        {
            int abattement;

            if(900000 < salaire * 0.3)
            {
                abattement = salaire * 30/100;
            }
            else
            {
                abattement = 900000;
            }

            return abattement;
        }

        public double CalculNbreParts(int conjoint, int nbrEnfant)
        {
            double nbParts;

            if(conjoint == 0)
            {
                nbParts = 1;
            }
            else
            {
                double nbPartsbeta = 2 + 0.5 * nbrEnfant;
                if(nbPartsbeta < 5)
                {
                    nbParts = nbPartsbeta;
                }
                else
                {
                    nbParts = 5;
                }
            }
            return nbParts;
        }

        public double Reduction(double nbParts, double IRPP)
        {
            double reduction = 0;

            switch (nbParts)
            {
                case 1: 
                    reduction = 0;
                    break;
                
                case 1.5:
                    double a = IRPP * 10 / 100;
                    if(a>100000 && a < 300000)
                    {
                        reduction += a;
                    }
                    else if (a < 100000)
                    {
                        reduction += 100000;
                    }
                    else
                    {
                        reduction += 300000;
                    }
                    break;

                case 2:
                    double b = IRPP * 15 / 100;
                    if (b > 200000 && b < 650000)
                    {
                        reduction += b;
                    }
                    else if (b < 200000)
                    {
                        reduction += 200000;
                    }
                    else
                    {
                        reduction += 650000;
                    }
                    break;

                case 2.5:
                    double c = IRPP * 20 / 100;
                    if (c > 300000 && c < 1100000)
                    {
                        reduction += c;
                    }
                    else if (c < 300000)
                    {
                        reduction += 300000;
                    }
                    else
                    {
                        reduction += 1100000;
                    }
                    break;

                case 3:
                    double d = IRPP * 25 / 100;
                    if (d > 400000 && d < 1650000)
                    {
                        reduction += d;
                    }
                    else if (d < 400000)
                    {
                        reduction += 400000;
                    }
                    else
                    {
                        reduction += 1650000;
                    }
                    break;

                case 3.5:
                    double e = IRPP * 30 / 100;
                    if (e > 500000 && e < 2030000)
                    {
                        reduction += e;
                    }
                    else if (e < 500000)
                    {
                        reduction += 500000;
                    }
                    else
                    {
                        reduction += 2030000;
                    }
                    break;

                case 4:
                    double f = IRPP * 35 / 100;
                    if (f > 600000 && f < 2490000)
                    {
                        reduction += f;
                    }
                    else if (f < 600000)
                    {
                        reduction += 600000;
                    }
                    else
                    {
                        reduction += 2490000;
                    }
                    break;

                case 4.5:
                    double g = IRPP * 40 / 100;
                    if (g > 700000 && g < 2755000)
                    {
                        reduction += g;
                    }
                    else if (g < 700000)
                    {
                        reduction += 700000;
                    }
                    else
                    {
                        reduction += 2755000;
                    }
                    break;

                case 5:
                    double h = IRPP * 45 / 100;
                    if (h > 800000 && h < 3180000)
                    {
                        reduction += h;
                    }
                    else if (h < 800000)
                    {
                        reduction += 800000;
                    }
                    else
                    {
                        reduction += 3180000;
                    }
                    break;
            }

            return reduction;
        }

        public double CalculImpot(double irpp, double reduction)
        {
            double impot = irpp - reduction;

            if (impot < 0)
            {
                impot = 0;
            }

            return impot;
        }

        public double CalculSalaireNet(double salaireAnnuel, double impot)
        {
            return salaireAnnuel - impot;
        }

    }
}
