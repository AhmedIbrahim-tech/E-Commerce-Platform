Context Name | Purpose | File | Usage locations
--- | --- | --- | ---
No custom Contexts found | Theme/Language are already using Redux inside `src/store/slices/appSlice.ts`. | N/A | N/A

No custom Next.js/React Context (like `createContext`) was found in the user-authored folders (`src/hooks`, `src/utils`, `src/layouts`). All UI toggles and Language states are currently managed by Redux Toolkit via `useAppSelector` and `useAppDispatch` pointing to `appSlice.ts`. Thus, the migration of a Context (like Language/UI toggle) is already complete in the provided starterkit.
