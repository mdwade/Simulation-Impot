using System;
using System.Collections;
using System.Collections.Generic;
using Gtk;
using Simulation_Impots;

public partial class MainWindow : Gtk.Window
{
    Gdk.Color green = new Gdk.Color(80, 80, 80);

    SortedList<string, Employe> listEmployees = new SortedList<string, Employe> ();
    Employe employe;

    public MainWindow() : base(Gtk.WindowType.Toplevel)
    {
        Build();
        this.image1.Pixbuf = new Gdk.Pixbuf("/home/mouhamed/Bureau/drapeau.jpg");
        this.image2.Pixbuf = new Gdk.Pixbuf("/home/mouhamed/Bureau/drapeau.jpg");

    }

    protected void OnDeleteEvent(object sender, DeleteEventArgs a)
    {
        Application.Quit();
        a.RetVal = true;
    }

    //Button valider
    protected void OnButton1Clicked(object sender, EventArgs e)
    {
        if(entry1.Text.Length != 0 && entry2.Text.Length != 0 && entry3.Text.Length != 0 && entry4.Text.Length != 0 && entry5.Text.Length != 0 && entry6.Text.Length != 0 && entry7.Text.Length != 0)
        {
            if (entry2.Text.GetType() != typeof(int) || entry3.GetType() != typeof(int) || entry5.GetType() != typeof(int) || entry6.GetType() != typeof(int))
            {
                if(int.Parse(entry3.Text) == 0 || int.Parse(entry3.Text) == 1)
                {
                        if(int.Parse(entry2.Text) >= 50000)
                        {
                            if (!listEmployees.ContainsKey(entry7.Text))
                            {
                                employe = new Employe();
                                employe.setNom(entry1.Text);
                                employe.setPrenom(entry4.Text);
                                employe.setsalaireBrut(int.Parse(entry2.Text));
                                employe.setNbreJours(int.Parse(entry5.Text));
                                employe.setConjoint(int.Parse(entry3.Text));
                                employe.setNbreEnfant(int.Parse(entry6.Text));
                                employe.setMatricule(entry7.Text);

                                listEmployees.Add(entry7.Text, employe);

                                MessageDialog md = new MessageDialog(null, DialogFlags.DestroyWithParent, MessageType.Info, ButtonsType.Ok, entry1.Text + " a été ajouté(e) !");
                                md.Run();
                                md.Destroy();

                                double brutFiscAnuel = employe.CalculBrutFiscAnnuel(int.Parse(entry2.Text), int.Parse(entry5.Text));
                                label3.Text = brutFiscAnuel.ToString();

                                int abattement = employe.CalculAbattement(int.Parse(entry2.Text));
                                label22.Text = abattement.ToString();

                                int brutFiscApresAbattement = employe.CalculBrutFiscApresAbattement(int.Parse(entry2.Text), abattement);
                                label20.Text = brutFiscApresAbattement.ToString();

                                double nbParts = employe.CalculNbreParts(int.Parse(entry3.Text), int.Parse(entry6.Text));
                                label23.Text = nbParts.ToString();

                                double IRPP = employe.CalculIRPP(int.Parse(entry2.Text));
                                label21.Text = IRPP.ToString();

                                double reduction = employe.Reduction(nbParts, IRPP);
                                label24.Text = reduction.ToString();

                                double impot = employe.CalculImpot(IRPP, reduction);
                                label25.Text = impot.ToString();

                                double salaireNet = employe.CalculSalaireNet(brutFiscAnuel, impot);
                                entry8.Text = "Salaire Net : " + salaireNet.ToString();
                            }
                            else
                            {
                                MessageDialog md = new MessageDialog(null, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok, "Cet employé existe déja !");
                                md.Run();
                                md.Destroy();
                            }
                            
                        }
                        else
                        {
                            MessageDialog md = new MessageDialog(null, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok, "Le salaire doit être supérieur à 50 000 !");
                            md.Run();
                            md.Destroy();
                        }
                }
                else
                {
                    MessageDialog md = new MessageDialog(null, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok, "Conjoint ne peut prendre que la valeur 0 ou 1 !");
                    md.Run();
                    md.Destroy();
                }

            }
            else
            {
                MessageDialog md = new MessageDialog(null, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok, "Veuillez vérifier que les toutes les valeurs sont correctes svp !");
                md.Run();
                md.Destroy();
            }

        }
        else
        {
            MessageDialog md = new MessageDialog(null, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Ok, "Veuillez remplir les champs svp !");
            md.Run();
            md.Destroy();
        }
    }

    //Button supprimer
    protected void OnButton2Clicked(object sender, EventArgs e)
    {
        entry1.DeleteText(0, 100);
        entry2.DeleteText(0, 100);
        entry3.DeleteText(0, 100);
        entry4.DeleteText(0, 100);
        entry5.DeleteText(0, 100);
        entry6.DeleteText(0, 100);
        entry7.DeleteText(0, 100);
    }

    //Button liste employés
    protected void OnButton3Clicked(object sender, EventArgs e)
    {

        Gtk.Window window = new Gtk.Window("Liste des employés");
        window.SetSizeRequest(1000, 500);

        Gtk.TreeView tree = new Gtk.TreeView();

        window.Add(tree);

        Gtk.TreeViewColumn matriculeColumn = new Gtk.TreeViewColumn();
        matriculeColumn.Title = "Matricule";

        Gtk.TreeViewColumn nomColumn = new Gtk.TreeViewColumn();
        nomColumn.Title = "Nom";

        Gtk.TreeViewColumn prenomColumn = new Gtk.TreeViewColumn();
        prenomColumn.Title = "Prénom";

        Gtk.TreeViewColumn salaireColumn = new Gtk.TreeViewColumn();
        salaireColumn.Title = "Salaire Brut";

        Gtk.TreeViewColumn NbreJourColumn = new Gtk.TreeViewColumn();
        NbreJourColumn.Title = "NbreJours";

        Gtk.TreeViewColumn conjointColumn = new Gtk.TreeViewColumn();
        conjointColumn.Title = "Conjoint";

        Gtk.TreeViewColumn nbreEnfantColumn = new Gtk.TreeViewColumn();
        nbreEnfantColumn.Title = "Enfant";

        tree.AppendColumn(matriculeColumn);


        Gtk.ListStore employeListe = new Gtk.ListStore(typeof(string));

        tree.Model = employeListe;
        ICollection key = (ICollection)listEmployees.Keys;

        foreach (string k in key)
        {
            if (listEmployees[k].getConjoint() ==0 )
            {

                employeListe.AppendValues(listEmployees[k].getNom()+" "+ listEmployees[k].getPrenom() + ", Célibataire, Salaire brut : " +listEmployees[k].getSalaireBrut());
            }
            else
            {
                Console.WriteLine(listEmployees[k].getNom() + " " + listEmployees[k].getPrenom() + ", Conjoint non salarié, Salaire brut : " + listEmployees[k].getSalaireBrut());

            }

        }

        Gtk.CellRendererText employeMatriculeCell = new Gtk.CellRendererText();
        matriculeColumn.PackStart(employeMatriculeCell, true);

        Gtk.CellRendererText employeNomCell = new Gtk.CellRendererText();
        nomColumn.PackStart(employeNomCell, true);

        Gtk.CellRendererText employePrenomCell = new Gtk.CellRendererText();
        prenomColumn.PackStart(employePrenomCell, true);

        Gtk.CellRendererText employeSalaireCell = new Gtk.CellRendererText();
        salaireColumn.PackStart(employeSalaireCell, true);

        Gtk.CellRendererText employeNbreJourCell = new Gtk.CellRendererText();
        NbreJourColumn.PackStart(employeNbreJourCell, true);

        Gtk.CellRendererText employeConjointCell = new Gtk.CellRendererText();
        conjointColumn.PackStart(employeConjointCell, true);

        Gtk.CellRendererText employeNbreEnfantCell = new Gtk.CellRendererText();
        nbreEnfantColumn.PackStart(employeNbreEnfantCell, true);


        // Tell the Cell Renderers which items in the model to display
        matriculeColumn.AddAttribute(employeMatriculeCell, "text", 0);
        nomColumn.AddAttribute(employeNomCell, "text", 0);
        prenomColumn.AddAttribute(employePrenomCell, "text", 0);
        salaireColumn.AddAttribute(employeSalaireCell, "text", 0);
        NbreJourColumn.AddAttribute(employeNbreJourCell, "text", 0);
        conjointColumn.AddAttribute(employeConjointCell, "text", 0);
        nbreEnfantColumn.AddAttribute(employeNbreEnfantCell, "text", 0);


        window.ShowAll();


    }

    //Button quitter l'application
    protected void OnButton4Clicked(object sender, EventArgs e)
    {
        Application.Quit();
    }


}
