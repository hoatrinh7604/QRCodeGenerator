using System.Globalization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ZXing;
using ZXing.QrCode;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject inputField;

    [SerializeField] TMP_InputField QRvalueText;
    [SerializeField] TMP_InputField QRSize;

    [SerializeField] Toggle useCustomLogo;
    [SerializeField] Button addLogoButton;
    [SerializeField] TextMeshProUGUI logoPath;

    [SerializeField] TextMeshProUGUI QRPathTitle;
    [SerializeField] TextMeshProUGUI QRPath;

    [SerializeField] RawImage QRResult;
    [SerializeField] RectTransform QRResultRect;

    [SerializeField] Button generateBtn;
    [SerializeField] Button saveBtn;

    [SerializeField] DataSaveController saveController;

    private int qrSize = 256;
    //Singleton
    public static Controller Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    public void OnValueChanged()
    {
        generateBtn.interactable = QRvalueText.text != "";
    }

    public void UpdateQRSize()
    {
        try
        {
            qrSize = int.Parse(QRSize.text);

            if(qrSize <= 0)
            {
                qrSize = 16;
                QRSize.text = "16";
            }
            else if (qrSize > 2048)
            {
                qrSize = 2048;
                QRSize.text = "2048";
            }

        }
        catch(System.Exception e)
        {

        }
    }

    private void Start()
    {
        QRPathTitle.gameObject.SetActive(false);
        QRPath.gameObject.SetActive(false);

        _storeEncodedTex = new Texture2D(256,256);
        useCustomLogo.onValueChanged.AddListener(delegate { IsUseCustomLogo(); });
        QRvalueText.onValueChanged.AddListener(delegate { OnValueChanged(); });

        QRSize.onDeselect.AddListener(delegate { UpdateQRSize(); });

        generateBtn.onClick.AddListener(delegate { GenerateQR(); });
        saveBtn.onClick.AddListener(delegate { SaveQR(); });

        QRSize.text = "256";
        qrSize = 256;
        saveBtn.interactable = false;
        generateBtn.interactable = false;
    }

    public void IsUseCustomLogo()
    {
        if(useCustomLogo.isOn)
        {
            addLogoButton.interactable = true;
            logoPath.gameObject.SetActive(true);
            logoPath.text = "";
        }
        else
        {
            addLogoButton.interactable = false;
            logoPath.gameObject.SetActive(false);
            logoPath.text = "";
        }
    }

    public void GenerateQR()
    {
        EncodeTextToQR();
        QRPathTitle.gameObject.SetActive(false);
        QRPath.gameObject.SetActive(false);

        if(QRResult.texture != null)
        {
            saveBtn.interactable = true;
        }
    }

    public void SaveQR()
    {
        var path = saveController.WriteFile(_storeEncodedTex);
        QRPath.text = path;

        QRPathTitle.gameObject.SetActive(true);
        QRPath.gameObject.SetActive(true);
    }

    private Color32 [] Encode(string textForEncoding, int width, int height)
    {
        BarcodeWriter writer = new BarcodeWriter
        {
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions
            {
                Height = height,
                Width = width
            }
        };

        return writer.Write(textForEncoding);
    }

    private Texture2D _storeEncodedTex;
    private void EncodeTextToQR()
    {
        Color32[] _convertPixel = Encode(QRvalueText.text, qrSize, qrSize);
        _storeEncodedTex.SetPixels32(_convertPixel);
        _storeEncodedTex.Apply();

        QRResult.texture = _storeEncodedTex;
        
    }

    public void Quit()
    {
        Application.Quit();
    }

}
