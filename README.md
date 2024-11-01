# QA_Veem
## Role:  Developer in QA_Veeam Software

### Achived poits:
- Synchronization must be one-way: after the synchronization content of the
replica folder should be modified to exactly match content of the source
folder;

- Synchronization should be performed periodically;

- File creation/copying/removal operations should be logged to a file and to the
console output;

- Folder paths, synchronization interval and log file path should be provided using
the command line arguments;

- It is undesirable to use third-party libraries that implement folder synchronization;
It is allowed (and recommended) to use external libraries implementing
other well-known algorithms. For example, there is no point in implementing yet
another function that calculates MD5 if you need it for the task – it is
perfectly acceptable to use a third-party (or built-in) library;

- The solution should be presented in the form of a link to the public GitHub repository.

### How it works:

- Souce and replica folder's path will be asked when console application startup. If replica folder does not exists console application will create it 
- All the information will always be available such as source path, replica path, and logs;
- Log structure will be
   `< operation >, < Name >, < Type of object >, < Date time start >, < Date time end>, < Memory space >, < Happy Ending  > `


