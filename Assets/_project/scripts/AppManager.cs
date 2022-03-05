using AWSSDK.Examples;
using UnityEngine;
using TMPro;
using System.IO;
using Amazon.S3.Model;

public class AppManager : MonoBehaviour
{
    #region VARIABLES

    [Header("general")]
    [SerializeField] private TMP_Text statusText;
    [Header("upload")]
    [SerializeField] private TMP_InputField pathFileInput;
    [SerializeField] private TMP_InputField bucketNameInput;
    [Header("get")]
    [SerializeField] private TMP_InputField fileOnBucketName;

    #endregion
    #region CREATE/UPLOAD FILE

    public void UploadFile()
    {
        statusText.text = "Uploading file...";
        string fileNameWithExtesion = Path.GetFileName(pathFileInput.text);
        S3Manager.Instance.UploadObjectForBucket(
            pathFileInput.text,
            bucketNameInput.text,
            fileNameWithExtesion,
            UploadFileCallback);
    }

    private void UploadFileCallback(PostObjectResponse postObjectResponse, string error)
    {
        if (string.IsNullOrEmpty(error))
        {
            statusText.text = "Upload with success";
        }
        else
        {
            statusText.text = $"Erro upload file {error}";
        }
    }

    #endregion

    #region READ/GET FILE

    public void GetFile()
    {
        statusText.text = "Get file...";
        S3Manager.Instance.GetObjectBucket(
            bucketNameInput.text, 
            fileOnBucketName.text, 
            GetFileCallback);
    }

    private void GetFileCallback(GetObjectResponse getObjectResponse, string error)
    {
        statusText.text = "Getting file...";
        if (string.IsNullOrEmpty(error))
        {
            string directoryPath = @"C:\";
            statusText.text = $"Success get file";
            Utils.ConvertObjectS3ForFile(
                getObjectResponse.ResponseStream,
                directoryPath,
                fileOnBucketName.text);
        } else
        {
            statusText.text = $"Error get file {error}";
        }
    }

    #endregion

    #region DELETE FILE

    public void DeleteFile()
    {
        statusText.text = "Delete file...";
        S3Manager.Instance.DeleteObjectOnBucket(
            fileOnBucketName.text,
            bucketNameInput.text,
            DeleteFileCallback);
    }

    private void DeleteFileCallback(DeleteObjectsResponse deleteObjectsResponse, string error)
    {
        statusText.text = "Deleting file...";
        if (string.IsNullOrEmpty(error))
        {
            statusText.text = $"Delete file with success";
        }
        else
        {
            statusText.text = $"Delete file {error}";
        }
    }

    #endregion
}
