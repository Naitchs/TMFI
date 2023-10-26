export interface Exceldatarow {
  rowNumber: number;
  columnData: { [key: string]: { value: string; isEditing: boolean; newValue: string } };
  sheetName: string;
}
