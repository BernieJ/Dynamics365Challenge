# Process

1. When development on a new release is started
    1. Add a column to the right hand side of the table below.
2. When making a customisation
    1. Add a row to the table below. Fill in the 
        1. Task: A link to the DevOps task. Tasks should be split to logically independent customisations.
        2. Component: The component that was customised (eg. Entity, Role, Process, etc)
        3. Customisation: a detailed description of the customisation. The description should be detailed enough to allow another developer without context of the change to correctly apply the change to another environment.
    2. Apply the customisation to the release environment.
    3. Fill in the date that the customisation was applied to the release environment.
    4. If the release is certain to be released in order:
        1. Apply the customisation to the environment(s) to the right of the release.
        2. Fill in the date that the customisation was applied to the environment(s).
3. When a release is deployed to production
    1. Fill in the date of deployment in the Deployed row for the release.
    2. If any customisations from the release have not been applied to any environments to the right of the release:
        1. Apply the customisation to the environment(s) to the right of the release.
        2. Fill in the date that the customisation was applied to the environment(s).

# Customisation Log

| Task | Component | Customisation | D01<br>CRM_2021_Q1_R2_Inspection,<br>Deployment Date: 19/03/2021 |
| :--- | :-------- | :------------ | --------------------------------------------------------------- |
|      |           |               |                                                                 |