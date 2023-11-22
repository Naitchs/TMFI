export interface Subjects {

  id: number;
  subjectCode: string;
  name: string;
  description: string;
  phase: PhaseEnum;

}

export enum PhaseEnum {
  Phase1 = 0,
  Phase2 = 1,
  Phase3 = 2,
  Phase4 = 3,
  Phase5 = 4,
  Phase6 = 5
}
