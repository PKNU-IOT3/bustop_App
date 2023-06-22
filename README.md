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

## 변경되어야할 항목
- 모바일 환경에서의 DB연결 문제를 해결해야함
- 현재 구현된 상태는 window machine으로 구동하여 MySQL과 연결된 상태

## 0613 수정
### MySQL -> RestAPI 연결 변경
- 기존에 MySQL 사용으로 인해 Android Amulator에서 연결 시 오류나는 부분 수정
- MySQL 대신 해당 DataBase를 API화 시켜 API와 연결 후 정보 출력 성공
<img src="https://raw.githubusercontent.com/PKNU-IOT3/bustop_app/main/images/0613_API_success.png" width=400 />

## 0622 수정
### 탑승 대기/취소 버튼 RestAPI와 연동
- MySQL과의 직접적인 DB 연결 문제를 해결하기 위해 API 사용
- 탑승 대기/취소 버튼 클릭 시 bus_cnt를 +1/-1
- 인원수 증/감과 동시에 버스 정보 재 출력
### 버스 정보 출력 버튼
- 버스 정보 출력 클릭 시 bool 함수를 이용하여 이미 정보가 출력되어 있는 경우 삭제

### 메인 화면
<img src="https://raw.githubusercontent.com/PKNU-IOT3/bustop_app/main/images/0622_MainPage.png" width=25% height=15% />

### 버스 정보 출력 버튼 클릭 시 버스 정보 출력 화면
<img src="https://raw.githubusercontent.com/PKNU-IOT3/bustop_app/main/images/0622_ShowItems.png" width=25% height=15% />

### 탑승 대기 / 탑승 취소 버튼 디자인 변경 및 기능 구현
<img src="https://raw.githubusercontent.com/PKNU-IOT3/bustop_app/main/images/0622_function.png" width=30% height=20% />

## 실행화면
<img src="https://raw.githubusercontent.com/PKNU-IOT3/bustop_app/main/images/0622_bustop.gif" width=400 />
