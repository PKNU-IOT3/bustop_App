# .Net MAUI를 활용한 bustop 애플리케이션
2023.05.16 제작

## 개발목적
- 사용자가 App을 활용하여 모바일 환경에서 특정 버스에 탑승하고자 함을 알리기 위함

## 기술스택
- C#
- .Net MAUI
- MySQL

## UI
- 선행 프로젝트였던 PyQT에 사용한 색상 및 형태를 그대로 차용<br>
- [Bustop_PyQT](https://github.com/PKNU-IOT3/bustop_PyQT)

## 로직
- .xaml , 데이터 바인딩을 활용하여 구현
- 버스의 탑승대기/탑승취소 버튼을 통해 해당 버스 탑승 인원 카운팅
- 카운팅된 인원을 DB로 저장

# 실행화면
## MainPage
<img src="https://raw.githubusercontent.com/PKNU-IOT3/bustop_app/main/images/0516_MainPage.png" width=400 />

## 탑승 대기 버튼
<img src="https://raw.githubusercontent.com/PKNU-IOT3/bustop_app/main/images/0516_Add.png" width=400 />

## 탑승 취소 버튼
<img src="https://raw.githubusercontent.com/PKNU-IOT3/bustop_app/main/images/0516_Minus.png" width=400 />

## 예외처리
- ### 탑승 인원 50명 초과
<img src="https://raw.githubusercontent.com/PKNU-IOT3/bustop_app/main/images/0516_over.png" width=400 />

- ### 탑승 인원 0명일 때 취소 버튼 사용 시
<img src="https://raw.githubusercontent.com/PKNU-IOT3/bustop_app/main/images/0516_zero.png" width=400 />
