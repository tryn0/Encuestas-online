# Encuestas-online
Visual Base .NET project for Desarrollo de Interfaces

# IDE
Visual Studio Community 2019

## Description
This project is for DI class. The functionality is:

- Load file with questions to answer.
- Ctrl + P (Configurar > Preguntas) for questions editor (Won't save in encuesta.txt), it will save questions on memory. Add, delete or just move between questions.
- Ctrl + I (Encuestas > Iniciar) start the poll, you can save your answers in any moment, click on Graba or Ctrl + G (Encuestas > Grabar). It will save the answers in respuestas.txt.
- If you select Encuestas > Resultados or Ctrl + R the app will show you analytics of the answers in memory, not from file.
- In the right part you can select the poll and the question to answer from the selected poll, then answer it below (RadioButton).
- Finally, Ctrl + E (Configurar > E) is an option only for configure the exit, you can configure it choosing your prefer option from Configurar aviso. You can choose one or both options.

### Advices
- Before use it edit URL location of the files encuestas.txt and respuestas.txt
- encuestas.txt have a pattern for load the questions: poll-question,question,question,...