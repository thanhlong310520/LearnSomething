using UnityEngine;

namespace FitGrid
{
    public class FitGridForMultiScreen : MonoBehaviour
    {

        public float offset = 0.35f;
        public int numRows = 5;
        public int numCols = 6;
        public Sprite sprite;
        public float paddingLeft =0.5f;
        public float paddingRight =0.5f;
        public float horizontalSpacing = 0.1f;
        public float verticalSpacing = 0.1f;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                Test();
            }
        }

        public void Test()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            GenerateSpriteGrid(numRows, numCols,sprite,paddingLeft,paddingRight,horizontalSpacing,verticalSpacing);
        }
        private void GenerateSpriteGrid(int numRows, int numColumns, Sprite defaultSprite, float paddingLeft,float paddingRight, float horizontalSpacing,float verticalSpacing)
        {
            if (defaultSprite == null)
            {
                Debug.LogError("CustomSpriteGridCreator: Default Sprite is not assigned!");
                return;
            }
            if (numRows <= 0 || numColumns <= 0)
            {
                Debug.LogError("CustomSpriteGridCreator: Number of rows and columns must be greater than 0.");
                return;
            }

            // Lấy kích thước thế giới của màn hình từ Camera Orthographic
            float screenHeightWorld = Camera.main.orthographicSize * 2f;
            float screenWidthWorld = screenHeightWorld * Camera.main.aspect;

            // --- Bước 1: Tính toán scaleXY dựa trên việc phủ đầy chiều ngang ---
            // Chiều rộng khả dụng cho các sprite sau khi trừ padding cố định
            float availableWidthForSprites = screenWidthWorld - paddingLeft - paddingRight;

            // Tổng khoảng cách ngang giữa các sprite
            float totalHorizontalSpacing = (numColumns - 1) * horizontalSpacing;

            // Tổng chiều rộng mà tất cả các sprite cần chiếm (sau khi trừ spacing)
            float widthNeededForAllSprites = availableWidthForSprites - totalHorizontalSpacing;

            // Chiều rộng mong muốn của MỘT sprite trong world units để vừa vặn chiều ngang
            float targetSpriteWidth = widthNeededForAllSprites / numColumns;

            // Lấy kích thước gốc của sprite trong world units
            float spriteOriginalWidthInWorldUnits = defaultSprite.rect.width / defaultSprite.pixelsPerUnit;
            float spriteOriginalHeightInWorldUnits = defaultSprite.rect.height / defaultSprite.pixelsPerUnit;

            // Tính toán scaleXY (giá trị scale chung cho cả X và Y)
            float scaleXY = targetSpriteWidth / spriteOriginalWidthInWorldUnits;

            // --- Bước 2: Tính toán chiều cao của sprite sau khi scale ---
            float scaledSpriteHeight = spriteOriginalHeightInWorldUnits * scaleXY;

            // Kiểm tra kích thước có hợp lệ không
            if (targetSpriteWidth <= 0 || scaledSpriteHeight <= 0)
            {
                Debug.LogError("Calculated sprite dimensions are zero or negative. Adjust padding/spacing values or check screen size.");
                return;
            }

            // --- Bước 3: Tính toán vị trí bắt đầu để căn giữa lưới ---
            // Tổng chiều rộng của toàn bộ lưới (bao gồm sprites và spacing)
            float totalGridWidth = (numColumns * targetSpriteWidth) + ((numColumns - 1) * horizontalSpacing);
            // Tổng chiều cao của toàn bộ lưới (bao gồm sprites và spacing)
            float totalGridHeight = (numRows * scaledSpriteHeight) + ((numRows - 1) * verticalSpacing);

            // Vị trí X của góc trên bên trái của lưới nếu nó được căn giữa
            // (0,0) là trung tâm màn hình, nên trừ đi nửa tổng chiều rộng lưới
            float gridLeftEdgeX = -totalGridWidth / 2f;
            // Vị trí Y của góc trên bên trái của lưới nếu nó được căn giữa
            // (0,0) là trung tâm màn hình, nên cộng thêm nửa tổng chiều cao lưới
            float gridTopEdgeY = totalGridHeight / 2f;

            // Vị trí X của trung tâm sprite đầu tiên (trên cùng bên trái)
            float startX = gridLeftEdgeX + targetSpriteWidth / 2f;
            // Vị trí Y của trung tâm sprite đầu tiên (trên cùng bên trái)
            float startY = gridTopEdgeY - scaledSpriteHeight / 2f;


            // --- Bước 4: Tạo và sắp xếp các sprite ---
            for (int row = 0; row < numRows; row++)
            {
                for (int col = 0; col < numColumns; col++)
                {
                    // Tính toán vị trí cho sprite hiện tại
                    Vector3 position = new Vector3(
                        startX + col * (targetSpriteWidth + horizontalSpacing),
                        startY - row * (scaledSpriteHeight + verticalSpacing),
                        0f // Vị trí Z mặc định cho 2D. Điều chỉnh nếu bạn cần chiều sâu
                    );

                    // Tạo Game Object mới
                    //GameObject spriteGO = new GameObject($"Sprite_{row}_{col}");
                    var spriteGO = new GameObject($"Sprite_{row}_{col}");
                    spriteGO.transform.parent = this.transform; // Đặt làm con của Game Object chứa script này
                    spriteGO.transform.position = position + new Vector3(0, offset, 0);

                    
                    

                    // Thêm Sprite Renderer
                    SpriteRenderer sr = spriteGO.GetComponent<SpriteRenderer>();
                    if (sr == null)
                    {
                        sr = spriteGO.AddComponent<SpriteRenderer>();
                    }
                    sr.sprite = defaultSprite;

                    // Áp dụng scale XY bằng nhau
                    spriteGO.transform.localScale = new Vector3(scaleXY, scaleXY, 1f);

                }
            }
        }
    }
}
