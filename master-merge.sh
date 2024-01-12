BRANCH=$(git symbolic-ref --short HEAD)

# Verifica se il branch è 'master'
if [ "$BRANCH" == "master" ]; then

    git merge --squash develop
    
    # Estrae il numero di versione dal file .csproj
    VERSION=$(grep -oP '<AssemblyVersion>[^<]*' Veesy.WebApp/Veesy.WebApp.csproj | sed 's/<\/\?AssemblyVersion>//g')

    if ! git rev-parse -q --verify $VERSION >/dev/null; then
      
        echo "Il tag $VERSION non esiste. Inizio il merge."
        
        # Commit manuale
        git commit -m "Versione $VERSION"
        
        git tag -a "$VERSION" -m "Versione $VERSION"
        
        git push origin $BRANCH --tags
        
    else
        prev_commit_hash=$(git rev-parse HEAD^)
        git reset --soft $prev_commit_hash
        echo "Il tag $VERSION esiste già. Non è stato creato nuovamente."
        exit 0
    fi
fi
read -p "Premi INVIO per chiudere la finestra..."