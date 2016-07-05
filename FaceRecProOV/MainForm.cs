

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.IO;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace MultiFaceRec
{
    public partial class FrmPrincipal : Form
    {
        //Declararation of all variables, vectors and haarcascades
        Image<Bgr, Byte> currentFrame;
        Capture grabber;
        HaarCascade face;
        HaarCascade eye;
        MCvFont font = new MCvFont(FONT.CV_FONT_HERSHEY_TRIPLEX, 0.5d, 0.5d);
        Image<Gray, byte> result, TrainedFace = null;
        Image<Gray, byte> gray = null;
        List<Image<Gray, byte>> trainingImages = new List<Image<Gray, byte>>();
        List<string> labels = new List<string>();
        List<string> NamePersons = new List<string>();
        int ContTrain, NumLabels, t;
        string name, names = null;
        int counter = 0;
        int k = 0;
        public int i = 0;
        Dictionary<string, int> dictionary = new Dictionary<string, int>();
        KeyValuePair<string, int> max = new KeyValuePair<string, int>();
        
       
        // public string connectionString = "Server=localhost;UserId=root;Password=;Database=atm;";

        public FrmPrincipal()
        {
            InitializeComponent();
            //Load haarcascades for face detection
            face = new HaarCascade("haarcascade_frontalface_default.xml");
            //eye = new HaarCascade("haarcascade_eye.xml");
            try
            {
                //Load of previus trainned faces and labels for each image
                string Labelsinfo = File.ReadAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt");
                string[] Labels = Labelsinfo.Split('%');
                NumLabels = Convert.ToInt16(Labels[0]);
                ContTrain = NumLabels;
                string LoadFaces;

                for (int tf = 1; tf < NumLabels + 1; tf++)
                {
                    LoadFaces = "face" + tf + ".bmp";
                    trainingImages.Add(new Image<Gray, byte>(Application.StartupPath + "/TrainedFaces/" + LoadFaces));
                    labels.Add(Labels[tf]);
                }

            }
            catch (Exception e)
            {
                //MessageBox.Show(e.ToString());
                MessageBox.Show("Nothing in binary database, please add at least a face(Simply train the prototype with the Add Face Button).", "Triained faces load", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }


        private void button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = false;
            //button3.Enabled = false;
            Timer timer = new Timer();
            timer.Tick += new EventHandler(timer1_Tick);
            timer.Interval = 1000;
            timer.Enabled = true;
            timer.Start();

            //Initialize the capture device
            grabber = new Capture();
            grabber.QueryFrame();
            //Initialize the FrameGraber event
            Application.Idle += new EventHandler(FrameGrabber);
            button1.Enabled = false;
        }


        private void button2_Click(object sender, System.EventArgs e)
        {


            try
            {

                //Trained face counter
                ContTrain = ContTrain + 1;

                //Get a gray frame from capture device
                gray = grabber.QueryGrayFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

                //Face Detector
                MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
                face,
                1.2,
                10,
                Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                new Size(20, 20));

                //Action for each element detected
                foreach (MCvAvgComp f in facesDetected[0])
                {
                    TrainedFace = currentFrame.Copy(f.rect).Convert<Gray, byte>();
                    break;
                }

                //resize face detected image for force to compare the same size with the 
                //test image with cubic interpolation type method
                TrainedFace = result.Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                trainingImages.Add(TrainedFace);
                labels.Add(textBox1.Text);

                //Show face added in gray scale
                imageBox1.Image = TrainedFace;

                //Write the number of triained faces in a file text for further load
                File.WriteAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", trainingImages.ToArray().Length.ToString() + "%");

                //Write the labels of triained faces in a file text for further load
                for (int i = 1; i < trainingImages.ToArray().Length + 1; i++)
                {
                    trainingImages.ToArray()[i - 1].Save(Application.StartupPath + "/TrainedFaces/face" + i + ".bmp");
                    File.AppendAllText(Application.StartupPath + "/TrainedFaces/TrainedLabels.txt", labels.ToArray()[i - 1] + "%");
                }

                MessageBox.Show(textBox1.Text + "´s face detected and added :)", "Training OK", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                MessageBox.Show("Enable the face detection first", "Training Fail", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            button2.Text = "TRAIN FACE";
            if (k == 0)
            {
                k++;
                int balance = int.Parse(textBox5.Text);
                string connectionString = "Server=localhost;UserId=root;Password=;Database=atm;";
                MySqlConnection cnn = new MySqlConnection(connectionString);
                try
                {
                    cnn.Open();
                    //string query = "INSERT INTO store VALUES (" + max.Key + ")";
                    string query = "insert into user(user_name,user_mob,user_pin,balance) values ('" + textBox1.Text + "','" + textBox3.Text + "','" + textBox4.Text + "','" + balance + "');";
                    MySqlCommand MyCommand2 = new MySqlCommand(query, cnn);  //This is command class which will handle the query and connection object.
                    MySqlDataReader MyReader2 = MyCommand2.ExecuteReader();  // Here our query will be executed and data saved into the database.
                    MessageBox.Show(query);
                    cnn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        void FrameGrabber(object sender, EventArgs e)
        {
            //label3.Text = "0";
            //label4.Text = "";
            NamePersons.Add("");


            //Get the current frame form capture device
            currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

            //Convert it to Grayscale
            gray = currentFrame.Convert<Gray, Byte>();

            //Face Detector
            MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
          face,
          1.2,
          10,
          Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
          new Size(20, 20));

            //Action for each element detected
            foreach (MCvAvgComp f in facesDetected[0])
            {
                t = t + 1;
                result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                //draw the face detected in the 0th (gray) channel with blue color
                currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);


                if (trainingImages.ToArray().Length != 0)
                {
                    //TermCriteria for face recognition with numbers of trained images like maxIteration
                    MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                    //Eigen face recognizer
                    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                       trainingImages.ToArray(),
                       labels.ToArray(),
                       3000,
                       ref termCrit);
                    if (counter < 20)
                    {
                        name = recognizer.Recognize(result);
                       // MessageBox.Show(name);
                        //int a = int.Parse(name);
                        if (name != null)
                            if (name != null)
                                if (name != null)
                        {
                            if (!dictionary.ContainsKey(name))
                            {
                                //  MessageBox.Show("hi");
                                dictionary.Add(name, 1);
                            }
                            else
                            {
                                //MessageBox.Show("hello");
                                dictionary[name]++;
                            }
                        }
                        textBox2.Text = textBox2.Text + name;

                        //Draw the label for each face detected and recognized
                        currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));
                    }
                }

                NamePersons[t - 1] = name;
                NamePersons.Add("");


                //Set the number of faces detected on the scene
                //label3.Text = facesDetected[0].Length.ToString();

                /*
                //Set the region of interest on the faces
                        
                gray.ROI = f.rect;
                MCvAvgComp[][] eyesDetected = gray.DetectHaarCascade(
                   eye,
                   1.1,
                   10,
                   Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                   new Size(20, 20));
                gray.ROI = Rectangle.Empty;

                foreach (MCvAvgComp ey in eyesDetected[0])
                {
                    Rectangle eyeRect = ey.rect;
                    eyeRect.Offset(f.rect.X, f.rect.Y);
                    currentFrame.Draw(eyeRect, new Bgr(Color.Blue), 2);
                }
                 */




            }
            t = 0;

            // Names concatenation of persons recognized
            //for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
            //{
            //    names = names + NamePersons[nnn] + ", ";
            //}
            //Show the faces procesed and recognized
            imageBoxFrameGrabber.Image = currentFrame;
            //label4.Text = names;
            //names = "";
            ////Clear the list(vector) of names
            //NamePersons.Clear();

        }

        void FrameGrabber1(object sender, EventArgs e)
        {
            // label3.Text = "0";
            //label4.Text = "";
            NamePersons.Add("");


            //Get the current frame form capture device
            currentFrame = grabber.QueryFrame().Resize(320, 240, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);

            //Convert it to Grayscale
            gray = currentFrame.Convert<Gray, Byte>();

            //Face Detector
            MCvAvgComp[][] facesDetected = gray.DetectHaarCascade(
          face,
          1.2,
          10,
          Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
          new Size(20, 20));

            //Action for each element detected
            foreach (MCvAvgComp f in facesDetected[0])
            {
                t = t + 1;
                result = currentFrame.Copy(f.rect).Convert<Gray, byte>().Resize(100, 100, Emgu.CV.CvEnum.INTER.CV_INTER_CUBIC);
                //draw the face detected in the 0th (gray) channel with blue color
                currentFrame.Draw(f.rect, new Bgr(Color.Red), 2);


                //if (trainingImages.ToArray().Length != 0)
                //{
                //    //TermCriteria for face recognition with numbers of trained images like maxIteration
                //    MCvTermCriteria termCrit = new MCvTermCriteria(ContTrain, 0.001);

                //    //Eigen face recognizer
                //    EigenObjectRecognizer recognizer = new EigenObjectRecognizer(
                //       trainingImages.ToArray(),
                //       labels.ToArray(),
                //       3000,
                //       ref termCrit);
                //    if (counter < 20)
                //    {
                //        name = recognizer.Recognize(result);
                //        //int a = int.Parse(name);
                //        if (!dictionary.ContainsKey(name))
                //        {
                //            dictionary.Add(name, 1);
                //        }
                //        else
                //        {
                //            dictionary[name]++;
                //        }




                //        textBox2.Text = textBox2.Text + name;

                //        //Draw the label for each face detected and recognized
                //        currentFrame.Draw(name, ref font, new Point(f.rect.X - 2, f.rect.Y - 2), new Bgr(Color.LightGreen));
                //    }
                //}

                //NamePersons[t - 1] = name;
                //NamePersons.Add("");


                ////Set the number of faces detected on the scene
                //label3.Text = facesDetected[0].Length.ToString();

                /*
                //Set the region of interest on the faces
                        
                gray.ROI = f.rect;
                MCvAvgComp[][] eyesDetected = gray.DetectHaarCascade(
                   eye,
                   1.1,
                   10,
                   Emgu.CV.CvEnum.HAAR_DETECTION_TYPE.DO_CANNY_PRUNING,
                   new Size(20, 20));
                gray.ROI = Rectangle.Empty;

                foreach (MCvAvgComp ey in eyesDetected[0])
                {
                    Rectangle eyeRect = ey.rect;
                    eyeRect.Offset(f.rect.X, f.rect.Y);
                    currentFrame.Draw(eyeRect, new Bgr(Color.Blue), 2);
                }
                 */



            }
            t = 0;

            //Names concatenation of persons recognized
           /* for (int nnn = 0; nnn < facesDetected[0].Length; nnn++)
            {
                names = names + NamePersons[nnn] + ", ";
            }
            //Show the faces procesed and recognized
            imageBoxFrameGrabber.Image = currentFrame;
            //label4.Text = names;
            //names = "";
            //Clear the list(vector) of names
            //NamePersons.Clear();
            */
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            
            //timer1.Interval = 10000;
            //MessageBox.Show("hi");
            counter++;

            //if (counter==20 )
            //{
            //    //int count1 = dictionary.Count;
            //    //MessageBox.Show(count1.ToString());
            //   // MessageBox.Show("hello");
            //}
            if (counter == 10)
            {
                //dictionary.Keys.Max();
                //MessageBox.Show(max.Key);
                timer1.Stop();
                timer1.Enabled = false;
                foreach (var kvp in dictionary)
                {
                   //MessageBox.Show(kvp.Key);
                    if (kvp.Value > max.Value)
                    {
                        
                        max = kvp;
                    }
                }
                MessageBox.Show(max.Key);
                //textBox2.Text = max.Key;

                // string[] seperated= textBox2.Text.Split(' ');
                //foreach ( var character in dictionary.Count))

                string connectionString = "Server=localhost;UserId=root;Password=;Database=atm;";
                MySqlConnection cnn = new MySqlConnection(connectionString);
                try
                {
                    cnn.Open();
                    String query;
                    
                    //if(max.Key != null)
                      //   query = "INSERT INTO store VALUES (max.Key)";
                    query = "UPDATE store SET face = '" + max.Key + "' ";
                    MySqlCommand MyCommand2 = new MySqlCommand(query, cnn);  //This is command class which will handle the query and connection object.
                    MySqlDataReader MyReader2 = MyCommand2.ExecuteReader();  // Here our query will be executed and data saved into the database.
                    MessageBox.Show(query);
                    cnn.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    //MessageBox.Show("Hello");
                    Process.Start("C:\\Users\\Sharad\\Documents\\Visual Studio 2012\\Projects\\ATM_Project\\ATM_Project\\bin\\Debug\\ATM_Project.exe");
                    this.Close();
                }

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            //button3.Enabled = false;
            button1.Enabled = false;
            groupBox1.Visible = true;
            grabber = new Capture();
            grabber.QueryFrame();
            Application.Idle += new EventHandler(FrameGrabber1);



        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void imageBox1_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void FrmPrincipal_Load(object sender, EventArgs e)
        {

        }

    }
}

